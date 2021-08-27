using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

#pragma warning disable
namespace CCM.Console.Api.Controllers.SysManager
{
    /// <summary>
    /// 上帝模式
    /// </summary>
    public class GodModeController : Controller
    {
        #region redis

        /// <summary>
        /// redis管理页面
        /// </summary>
        /// <returns>页面</returns>
        public IActionResult Index()
        {
            return View("~/Views/GodMode/Index.html");
        }

        /// <summary>
        /// redis管理页面
        /// </summary>
        /// <returns>页面</returns>
        public IActionResult RedisHome()
        {
            return View("~/Views/GodMode/RedisHome.html");
        }

        #region api

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisDataType">类型</param>
        /// <returns>json </returns>
        [HttpGet]
        public JsonResult GetValue(string redisKey, string redisDataType)
        {
            var redis = ServiceHelperProvider.Instance.CacheHelperService;
            var jsonString = string.Empty;
            switch (redisDataType.PackInt())
            {
                case (int)RedisDataTypeEnum.List:
                    break;
                case (int)RedisDataTypeEnum.Set:
                    break;
                case (int)RedisDataTypeEnum.SortSet:
                    break;
                default:
                    var result = redis.Get<dynamic>(redisKey);
                    jsonString = JsonConvert.SerializeObject(result);
                    break;
            }
            var ttl = redis.KeyTTL(redisKey);
            return OkResult(
                new
                {
                    RedisValue = jsonString,
                    TTL = ttl
                });
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>json</returns>
        [HttpPost]
        public JsonResult SetValue(RedisViewModel model)
        {
            var redis = ServiceHelperProvider.Instance.CacheHelperService;
            var valueString = model.RedisValue;

            switch (model.RedisDataType.PackInt())
            {
                case (int)RedisDataTypeEnum.List:
                    break;
                case (int)RedisDataTypeEnum.Set:
                    // tod
                    break;
                case (int)RedisDataTypeEnum.SortSet:
                    // todo
                    break;
                case (int)RedisDataTypeEnum.HashTable:
                    // todo
                    break;
                default:
                    var a = JsonConvert.DeserializeObject<dynamic>(valueString);
                    redis.Set(model.RedisKey, (int)Math.Floor(model.TimeOut.PackDecimal()), a);
                    break;
            }

            return OkResult(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public JsonResult DeleteKey(string redisKey)
        {
            var redis = ServiceHelperProvider.Instance.CacheHelperService;
            redis.DelCache(redisKey);
            if (true)
            {
                return OkResult(string.Empty);
            }

            return FailResult("删除失败");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public JsonResult SetTTL( string redisKey, long timeout)
        {
            var redis = ServiceHelperProvider.Instance.CacheHelperService;
            if (redis.KeyExpire(redisKey, timeout))
            {
                return OkResult("");
            }

            return FailResult("设置失败");
        }

        #endregion
        #endregion

        #region 类和枚举

        /// <summary>
        /// 成功返回结果
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="fss">body</param>
        /// <param name="msg">信息</param>
        /// <returns>结果</returns>
        protected JsonResult OkResult<T>(T fss, string msg = "")
        {
            var resultMessage = new { ResultCode = 1, ResultMsg = msg ?? "成功", Body = fss };
            return Json(resultMessage);
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="fss">body</param>
        /// <param name="msg">信息</param>
        /// <returns>结果</returns>
        protected JsonResult FailResult<T>(T fss, string msg = "")
        {
            var resultMessage = new { ResultCode = 2, ResultMsg = msg ?? "失败", Body = fss };
            return Json(resultMessage);
        }

        /// <summary>
        /// RedisViewModel
        /// </summary>
        public class RedisViewModel
        {
            /// <summary>
            /// RedisKey
            /// </summary>
            public string RedisKey { get; set; }

            /// <summary>
            /// RedisValue
            /// </summary>
            public string RedisValue { get; set; }

            /// <summary>
            /// TimeOut
            /// </summary>
            public string TimeOut { get; set; }

            /// <summary>
            /// 0 对象 1 列表
            /// </summary>
            public RedisDataTypeEnum RedisDataType { get; set; }
        }

        /// <summary>
        /// RedisDataType
        /// </summary>
        public enum RedisDataTypeEnum
        {
            /// <summary>
            /// String
            /// </summary>
            String = 1,

            /// <summary>
            /// List
            /// </summary>
            List = 2,

            /// <summary>
            /// Set
            /// </summary>
            Set = 3,

            /// <summary>
            /// SortSet
            /// </summary>
            SortSet = 4,

            /// <summary>
            /// HashTable
            /// </summary>
            HashTable = 5,
        }
        #endregion
    }
}
