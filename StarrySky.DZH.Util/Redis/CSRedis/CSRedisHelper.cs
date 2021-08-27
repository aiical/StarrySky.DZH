using CSRedis;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Redis.CSRedis
{
    public class CSRedisHelper
    {
        static CSRedisClient redisClient = null;

        static CSRedisClient GetClient()
        {
            return redisClient;
        }
        static CSRedisHelper()
        {
            var redisConnStr = string.Empty;
            redisClient = new CSRedisClient(redisConnStr);      //Redis的连接字符串
            RedisHelper.Initialization(redisClient);
        }


        public static void SetStr<T>(string key, T value, double expiredSeconds)
        {
            RedisHelper.Set(key, value, expiredSeconds.PackInt());
        }

        public static T GetStr<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        /// <summary>
        /// 设置hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetHash(string key, string field, string value)
        {
            try
            {
                GetClient().HSet(key, field, value);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据表名，键名，获取hash值
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="field">键名</param>
        /// <returns></returns>
        public static string GetHash(string key, string field)
        {
            string result = "";
            try
            {

                result = GetClient().HGet(key, field);
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
    }
}
