using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace StarrySky.DZH.Util.Cache
{
    /// <summary>
    /// 内存缓存--单台服务器内存，不一样的服务器，不一样的缓存
    /// <para>Author:丁振华</para>
    /// </summary>
    public class MemoryCacheHelper
    {
        #region 内存缓存
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="objObject">缓存内容</param>
        /// <param name="timeout">默认一个小时，单位秒</param>
        /// <param name="hours">默认一个小时，</param>
        public static void SetCache(string cacheKey, object objObject, int timeout = 3600, int hours = 1)
        {
            try
            {
                if (objObject == null) return;
                var objCache = HttpRuntime.Cache;
                //绝对过期时间  
                objCache.Insert(cacheKey, objObject, null, DateTime.Now.AddSeconds(timeout * hours),
                    TimeSpan.Zero, CacheItemPriority.High, null);
            }
            catch (Exception)
            {
                //throw;  
            }
        }

        /// <summary>  
        /// 获取数据缓存  
        /// </summary>  
        /// <param name="cacheKey">键</param>  
        public static object GetCache(string cacheKey)
        {
            var objCache = HttpRuntime.Cache.Get(cacheKey);
            return objCache;
        }

        /// <summary>  
        /// 删除数据缓存  
        /// </summary>  
        /// <param name="cacheKey">键</param>  
        public static object DelCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
            return true;
        }
        #endregion
    }
}
