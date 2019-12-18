using Newtonsoft.Json;
using StackExchange.Redis;
using StarrySky.DZH.Util.DataConvert;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Redis
{
    public static class RedisHelper
    {
        /// <summary>
        /// 数据库
        /// </summary>
        private static IDatabase _db;
        private static IConnectionMultiplexer _connMultiplexer;
        static RedisHelper()
        {
            _connMultiplexer = RedisInstance.GetConnectionRedisMultiplexer();
            _db = RedisInstance.GetConnectionRedisMultiplexer().GetDatabase();
        }

        #region 内部方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<IServer> GetServers()
        {
            try
            {
                var list = new List<IServer>();
                var endPoints = _connMultiplexer.GetEndPoints();

                for (var index1 = 0; index1 < endPoints.Length; ++index1)
                {
                    list.Add(_connMultiplexer.GetServer(endPoints[index1].ToString()));
                }

                return list;
            }
            catch (Exception)
            {
                ////RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="database"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static IEnumerable<RedisKey> Keys(IServer server, int database = 0, RedisValue pattern = default(RedisValue),
            int pageSize = 10)
        {
            try
            {
                return server.Keys(database, pattern, pageSize);
            }
            catch (Exception)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return new List<RedisKey>();
            }

        }
        #endregion 


        #region key 的命令

        /// <summary>
        /// 返回匹配的key列表
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [Obsolete("由于需要在集群的所有机器上循环遍历符合特征的key值,此方法很可能会造成redis堵塞，生产环境请谨慎调用")]
        public static IEnumerable<string> KeyScan(string pattern)
        {
            var servers = GetServers();
            if (servers == null && !servers.Any())
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                yield break;
            }
            foreach (var server in servers)
            {
                foreach (var redisKey in Keys(server, 0, pattern, 100))
                {
                    yield return redisKey;
                }
            }
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDelete(string key)
        {
            try
            {
                return _db.KeyDelete(key);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "KeyDelete", LogMessageType.Error, ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// 判断key 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            try
            {
                return _db.KeyExists(key);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "KeyExists", LogMessageType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// key设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, long seconds)
        {
            try
            {
                return _db.KeyExpire(key, TimeSpan.FromSeconds(seconds));
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "KeyExpire", LogMessageType.Error, ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static double KeyTTL(string key)
        {
            try
            {
                var time = _db.KeyTimeToLive(key);
                return time?.TotalSeconds ?? 0;
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "KeyTTL", LogMessageType.Error, ex.ToString());
                return 0;
            }
        }
        #endregion

        #region string 的 命令

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        public static void Set<T>(string key, long seconds, T value)
        {
            Set<T>(key, (double)seconds, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        public static void Set<T>(string key, double seconds, T value)
        {
            try
            {
                _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(seconds));
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "Set", LogMessageType.Error, ex.ToString(),key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            try
            {
                var obj = _db.StringGet(key);
                if (obj == RedisValue.EmptyString || obj == RedisValue.Null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(obj);

            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "Get", LogMessageType.Error, ex.ToString(), key);
                return default(T);
            }
        }

        /// <summary>
        /// StringIncrBy（调用此方法缓存不会过期,请在业务逻辑合适的地方指定过期时间或者移除key）
        /// <para>将 key 所储存的值加上增量 increment 。</para>
        /// <para>如果 key 不存在，那么 key 的值会先被初始化为 0 ，然后再执行 StringIncrBy 命令。</para>
        /// <para>如果值包含错误的类型，或字符串类型的值不能表示为数字，那么返回一个错误。</para>
        /// <para>本操作的值限制在 64 位(bit)有符号数字表示之内。</para>
        /// <para>关于递增(increment) / 递减(decrement)操作的更多信息，参见 INCR 命令。</para>
        /// <para>时间复杂度：O(1)</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="increment"></param>
        /// <returns>返回值：加上 increment 之后， key 的值。</returns>
        public static long StringIncrBy(string key, long increment)
        {
            try
            {
                return _db.StringIncrement(key, increment);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "StringIncrBy", LogMessageType.Error, ex.ToString(), key);
                return -1;
            }
        }

        /// <summary>
        /// 设置redis缓存（已经有值会返回失败）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key值</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">设置过期时间</param>
        /// <returns></returns>
        public static bool RedisSetNx<T>(string key, T value, int seconds)
        {
            try
            {
                return _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(seconds),
                    When.NotExists);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "RedisSetNx", LogMessageType.Error, ex.ToString(),key);
                return false;
            }
        }

        /// <summary>
        /// 设置redis缓存（已经有值会返回失败）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key值</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">设置过期时间</param>
        /// <returns></returns>
        public static bool RedisSetNx<T>(string key, T value, double seconds)
        {
            try
            {
                return _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(seconds),
                    When.NotExists);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "RedisSetNx", LogMessageType.Error, ex.ToString(), key);
                return false;
            }
        }


        /// <summary>
        /// redis锁升级版， 获取到false后还需要判断第二个值是否为1,
        /// 1的为异常情况，此时锁判断不可用，需要业务自行逻辑验重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static (bool, int) RedisSetNxUpgrade<T>(string key, T value, int seconds)
        {
            try
            {
                return (_db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(seconds),
                    When.NotExists), 0);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return (false, 1);
            }
        }

        #endregion

        #region HashTable
        /// <summary>
        /// Redis散列数据类型  批量新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashEntryList"></param>
        public static void HashSet(string key, List<HashEntry> hashEntryList)
        {
            try
            {
                _db.HashSet(key, hashEntryList.ToArray());
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashSet", LogMessageType.Error, ex.ToString());
            }
        }

        /// <summary>
        /// Redis散列数据类型  新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="val"></param>
        public static void HashSet<T>(string key, string field, T val)
        {
            try
            {

                _db.HashSet(key, field, JsonConvert.SerializeObject(val));
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashSet", LogMessageType.Error, ex.ToString());
            }
        }

        /// <summary>
        ///  Redis散列数据类型 获取指定key的指定field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string field)
        {
            try
            {
                var obj = _db.HashGet(key, field);
                if (obj == RedisValue.EmptyString || obj == RedisValue.Null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(obj);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashGet", LogMessageType.Error, ex.ToString());
                return default(T);
            }
        }

        /// <summary>
        ///  Redis散列数据类型 获取所有field所有值,以 HashEntry[]形式返回
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HashEntry[] HashGetAll(string key)
        {
            try
            {
                return _db.HashGetAll(key);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashGetAll", LogMessageType.Error, ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Redis散列数据类型  单个删除field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string hashField)
        {
            try
            {
                return _db.HashDelete(key, hashField);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashDelete", LogMessageType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///  Redis散列数据类型  批量删除field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public static long HashDelete(string key, string[] hashFields)
        {
            try
            {
                return _db.HashDelete(key, hashFields.Select(t => (RedisValue)t).ToArray());
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashDelete", LogMessageType.Error, ex.ToString());
                return -1;
            }
        }

        /// <summary>
        ///  Redis散列数据类型 判断指定键中是否存在此field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string field)
        {
            try
            {
                return _db.HashExists(key, field);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashExists", LogMessageType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Redis散列数据类型  获取指定key中field数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long HashLength(string key)
        {
            try
            {
                return _db.HashLength(key);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashLength", LogMessageType.Error, ex.ToString());
                return -1L;
            }
        }

        /// <summary>
        /// Redis散列数据类型  为key中指定field增加incrVal值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="incrVal"></param>
        /// <returns></returns>
        public static double HashIncrement(string key, string field, double incrVal)
        {
            try
            {
                return _db.HashIncrement(key, field, incrVal);
            }

            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashIncrement", LogMessageType.Error, ex.ToString());
                return -1L;
            }
        }

        /// <summary>
        /// 将实体转换为redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="p"></param>
        /// <param name="seconds"></param>
        public static void HashSetFromModel<T>(string key, T p, long seconds = -1) where T : new()
        {
            try
            {
                if (p == null) return;

                var type = p.GetType();
                if (type.IsArray || type.IsGenericType || type.IsEnum)
                {
                    return;
                }

                foreach (var info in type.GetProperties())
                {
                    _db.HashSet(key, info.Name, JsonConvert.SerializeObject(info.GetValue(p, null)));
                }

                if (seconds > 0)
                {
                    _db.KeyExpire(key, TimeSpan.FromSeconds(seconds));
                }

            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashSetFromModel", LogMessageType.Error, ex.ToString());
                return;
            }
        }

        /// <summary>
        /// redis 转 实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static T HashGetToModel<T>(string key) where T : new()
        {
            try
            {
                var p = new T();
                var values = _db.HashGetAll(key);

                var type = p.GetType();
                var fields = type.GetProperties();
                foreach (var item in values)
                {

                    var fieldInfo = fields.FirstOrDefault(x => x.Name == item.Name.PackString());
                    if (fieldInfo == null) continue;

                    if (fieldInfo.PropertyType == typeof(string))
                    {
                        fieldInfo.SetValue(p, item.Value.PackString().Trim('\"'));
                    }
                    else if (fieldInfo.PropertyType == typeof(int))
                    {
                        fieldInfo.SetValue(p, item.Value.PackInt());
                    }
                    else if (fieldInfo.PropertyType == typeof(DateTime))
                    {
                        fieldInfo.SetValue(p, item.Value.PackDateTime());
                    }
                    else if (fieldInfo.PropertyType == typeof(long))
                    {
                        fieldInfo.SetValue(p, item.Value.PackLong());
                    }
                    else if (fieldInfo.PropertyType == typeof(decimal))
                    {
                        fieldInfo.SetValue(p, item.Value.PackDecimal(4));
                    }
                    else
                    {
                        var value = Convert.ChangeType(item.Value, fieldInfo.PropertyType);
                        fieldInfo.SetValue(p, value);
                    }
                }

                return p;
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "HashGetToModel ", LogMessageType.Error, ex.ToString());
                return default(T);
            }
        }

        #endregion

        #region List
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        public static void ListSet<T>(string key, List<T> list, long seconds = -1)
        {
            try
            {
                _db.KeyDelete(key);
                _db.ListRightPush(key, list.Select(x => (RedisValue)x.PackJson()).ToArray());
                if (seconds > 0)
                {
                    _db.KeyExpire(key, TimeSpan.FromSeconds(seconds));
                }
                return;
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return;
            }
        }

        /// <summary>
        /// 设置list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="list">list</param>
        /// <returns></returns>
        public static long ListRightPush<T>(string key, List<T> list)
        {
            return _db.ListRightPush(key, list.Select(x => (RedisValue)x.PackJson()).ToArray());
        }

        /// <summary>
        /// 设置list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static long ListRightPush<T>(string key, T list)
        {
            return _db.ListRightPush(key, list.PackJson());
        }

        /// <summary>
        /// 队列长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListLength(string key)
        {
            return _db.ListLength(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string key)
        {
            try
            {
                var obj = _db.ListRightPop(key);
                if (obj == RedisValue.EmptyString || obj == RedisValue.Null)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string key)
        {
            try
            {
                var obj = _db.ListLeftPop(key);
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return default(T);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(string key, T obj)
        {
            try
            {
                return _db.ListLeftPush(key, obj.PackJson());
            }
            catch
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return 0;
            }
        }

        /// <summary>
        /// 获取list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T ListGet<T>(string key, long index)
        {
            var flag = _db.ListGetByIndex(key, index);
            return JsonConvert.DeserializeObject<T>(flag);
        }

        /// <summary>
        /// 获取list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<T> ListGet<T>(string key)
        {
            try
            {
                var flag = _db.ListRange(key);
                return flag.Select(x => JsonConvert.DeserializeObject<T>(x));
            }
            catch (Exception ex)
            {
                //SkyLogHelper.Write("SystemError", "RedisHelper", "ListGet", LogMessageType.Error, ex.ToString());
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return default(List<T>);
            }

        }

        /// <summary>
        /// 获取list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IEnumerable<T> ListRange<T>(string key, int start, int stop)
        {
            try
            {
                var flag = _db.ListRange(key, start, stop);
                return flag.Select(x => JsonConvert.DeserializeObject<T>(x));
            }
            catch
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                return default(List<T>);
            }
        }

        /// <summary>
        /// 获取list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static void ListTrim(string key, int start, int stop)
        {
            try
            {
                _db.ListTrim(key, start, stop);
            }
            catch
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
            }
        }

        /// <summary>
        /// 移除一个相同的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ListRemove<T>(string key, T value)
        {
            return _db.ListRemove(key, value.PackJson());
        }

        /// <summary>
        /// 重写list的其中一个item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static long ListRewrite<T>(string key, T value, Func<T> func)
        {
            ListRemove(key, value);
            var r = func();
            return ListRightPush(key, r);
        }

        #endregion

        #region Set

        /// <summary>
        /// 向set里插入一个数(1 插入成功  0 已存在数据插入失败  -1 redis连接失败)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        public static (int code, string msg) SetAdd<T>(string key, T value, long seconds = -1)
        {
            try
            {
                var flag = _db.SetAdd(key, JsonConvert.SerializeObject(value));

                if (seconds > 0)
                {
                    _db.KeyExpire(key, TimeSpan.FromSeconds(seconds));
                }

                return (flag ? 1 : 0, "");
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SetAdd", LogMessageType.Error, ex.ToString(),key);
                return (-1, "");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> SetMembers<T>(string key)
        {
            try
            {
                var list = _db.SetMembers(key);
                if (list == null || !list.Any())
                {
                    return new List<T>();
                }
                return list.Select(x => JsonConvert.DeserializeObject<T>(x)).ToList();
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SetMembers", LogMessageType.Error, ex.ToString(), key);
                return new List<T>();
            }
        }
        /// <summary>
        /// 向set里插入一个数(1 插入成功  0 已存在数据插入失败  -1 redis连接失败)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetRemove<T>(string key, T value)
        {
            try
            {
                return _db.SetRemove(key, JsonConvert.SerializeObject(value));
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SetRemove", LogMessageType.Error, ex.ToString(), key);
                return false;
            }
        }
        /// <summary>
        /// 从set中移除并返回一个随机元素,如果key不存在或者set没有数据 返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T SetPop<T>(string key)
        {
            try
            {
                var obj = _db.SetPop(key);
                if (obj == RedisValue.EmptyString || obj == RedisValue.Null)
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SetPop", LogMessageType.Error, ex.ToString(),key);
                return default(T);
            }
        }

        /// <summary>
        /// 重写set的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool SetRewrite<T>(string key, T value, Func<T> func)
        {
            if (SetRemove(key, value))
            {
                var f = func();
                SetAdd(key, f);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SetLength(string key)
        {
            try
            {
                return _db.SetLength(key);
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SetLength", LogMessageType.Error, ex.ToString(), key);
                return 0;
            }
        }
        #endregion

        #region SortedSet 操作

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(string redisKey, string member, double score, long seconds = -1)
        {
            try
            {
                var flag = _db.SortedSetAdd(redisKey, member, score);
                if (seconds > 0)
                {
                    _db.KeyExpire(redisKey, TimeSpan.FromSeconds(seconds));
                }

                return flag;
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SortedSetAdd", LogMessageType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(string redisKey, T member, double score, long seconds = -1)
        {
            try
            {
                var json = member.PackJson();
                var flag = _db.SortedSetAdd(redisKey, json, score);
                if (seconds > 0)
                {
                    _db.KeyExpire(redisKey, TimeSpan.FromSeconds(seconds));
                }

                return flag;
            }
            catch (Exception ex)
            {
                //RedisConnectionHelper.RemoveConnectionMultiplexer(_groupName);
                //SkyLogHelper.Write("SystemError", "RedisHelper", "SortedSetAdd", LogMessageType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 在有序集合中返回元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> GetSortedSet(string redisKey)
        {
            return _db.SortedSetRangeByRank(redisKey);
        }
        /// <summary>
        ///  在有序集合中返回指定范围的元素。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> SortedSetRangeByRank(string redisKey, int start, long stop)
        {
            return _db.SortedSetRangeByRank(redisKey, start, stop);
        }
        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long SortedSetLength(string redisKey)
        {
            return _db.SortedSetLength(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static bool SortedSetRemove(string redisKey, string memebr)
        {
            return _db.SortedSetRemove(redisKey, memebr);
        }




        #endregion SortedSet 操作
    }
}
