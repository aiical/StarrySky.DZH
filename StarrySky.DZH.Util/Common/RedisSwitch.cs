using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    /// <summary>
    /// Redis缓存控制开关
    /// </summary>
    public class RedisSwitch
    {

        private static RedisSwitch _instance = null;
        /// <summary>
        /// 
        /// </summary>
        public static readonly object Locker = new object();
        private RedisSwitch() { }
        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static RedisSwitch GetInstance()
        {
            if (_instance == null)
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new RedisSwitch();
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 是否读缓存 true 读缓存 false 不读缓存
        /// </summary> 
        public bool IsReadCache
        {
            get
            {
                //1不读缓存
                return 0 != 1;
            }
        }


    }
}
