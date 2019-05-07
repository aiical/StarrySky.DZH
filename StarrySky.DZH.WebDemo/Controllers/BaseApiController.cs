using StarrySky.DZH.WebDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StarrySky.DZH.WebDemo.Controllers
{
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="fun"></param>
        /// <param name="request"></param>
        /// <param name="concurrencyKey">防并发锁，为空则不锁</param>
        /// <param name="concurrencySecond">并发锁秒数</param>
        /// <returns></returns>
        protected static ResultMessage<R> ExecuteConcurrencyFuc<T, R>(Func<T, ResultMessage<R>> fun, T request, string concurrencyKey, int concurrencySecond)
        {
            var response = Activator.CreateInstance<ResultMessage<R>>(); //default(ResultMessage<R>);
            try
            {
                if (request == null)
                {
                    response.ResultCode = 2;
                    response.ResultMsg = "请求参数不能为空";
                }
                else
                {
                    //参数检验，环境判断
                    //if (new[] { "product", "stage" }.Contains(Kernel.Environment))
                    if (!string.IsNullOrWhiteSpace(concurrencyKey) && concurrencySecond > 0)
                    {
                        var redisKey = $"{concurrencyKey}";
                        //if (!RedisCache.RedisSetNx(redisKey, "Lock", concurrencySecond))
                        //{
                        //    response.ResultCode = 2;
                        //    response.ResultMsg = "操作太快了,请稍后重试";
                        //    return response;
                        //}
                        //response = fun(request);
                        //if (response == null || response.ResultCode != 1) //如果处理失败，则进行释放缓存
                        //{
                        //    RedisCache.RedisDel(redisKey);
                        //}
                    }
                    else
                    {
                        response = fun(request);
                    }
                }
                return response;
            }
            catch (Exception ex) { }
            return response;
        }
    }
}
