using Newtonsoft.Json;
using StackExchange.Redis;
using StarrySky.DZH.Util.DataConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Redis
{
    /// <summary>
    /// redis连接工具
    /// neget包 StackExchange.Redis
    /// </summary>
    public class RedisInstance
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string redisConnStr = "127.0.0.1:6379,defaultDatabase=1,password=mypassword";


        /// <summary>
        /// ConnectionMultiplexer对象是StackExchange.Redis最中枢的对象。这个类的实例需要被整个应用程序域共享和重用的，你不要在每个操作中不停的创建该对象的实例,所以使用单例来创建和存放这个对象是必须的
        /// </summary>
        private static IConnectionMultiplexer _connMultiplexer;

        /// <summary>
        /// 私有化
        /// </summary>
        private RedisInstance()
        {
        }

        /// <summary>
        /// 获取 Redis 连接对象
        /// </summary>
        /// <returns></returns>
        public static IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            try
            {
                if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                {
                    lock (Locker)
                    {
                        if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                        {
                            _connMultiplexer = ConnectionMultiplexer.Connect(redisConnStr);
                            AddRegisterEvent();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _connMultiplexer;
        }

        #region 注册事件

        /// <summary>
        /// 添加注册事件
        /// </summary>
        private static void AddRegisterEvent()
        {
            _connMultiplexer.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            _connMultiplexer.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            _connMultiplexer.ErrorMessage += ConnMultiplexer_ErrorMessage;
            _connMultiplexer.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            _connMultiplexer.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            _connMultiplexer.InternalError += ConnMultiplexer_InternalError;
            _connMultiplexer.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }

        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            var log = $"InternalError:Message:{e.Exception.ToString()}";
            Console.WriteLine($"{nameof(ConnMultiplexer_InternalError)}: {e.Exception}");
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            var log = $"HashSlotMoved:NewEndPoint:{e.NewEndPoint},OldEndPoint:{e.OldEndPoint}";
            Console.WriteLine(
                $"{nameof(ConnMultiplexer_HashSlotMoved)}: {nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint}, ");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChanged)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            var log = $"InternalError:Message:{e.Message}";
            Console.WriteLine($"{nameof(ConnMultiplexer_ErrorMessage)}: {e.Message}");
        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            var eMessage = e.Exception == null ? "" : ", " + e.Exception.Message;
            var log = $"重新连接：Endpoint failed:{e.EndPoint},{e.FailureType}{eMessage}";
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionFailed)}: {e.Exception}");
        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }

        #endregion 注册事件    


    }
}
