using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace StarrySky.DZH.WebDemo.CustomAttribute
{
    /// <summary>
    /// Action执行会触发（绑定了这个）
    /// </summary>
    public class MyActionFilterAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            string memberId = "";
            string jsonString = "";

            try
            {
                //执行签到
                // var loginRequest = actionExecutedContext.ActionContext.ActionArguments.Values.FirstOrDefault() as LoginRequest;
                jsonString = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                //ResultMessage<WeiXinMemberInfoResponse> response = JsonConvert.DeserializeObject<ResultMessage<WeiXinMemberInfoResponse>>(jsonString);

                return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            }
            catch (Exception ex)
            {

            }
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }


        /// <summary>
        /// 读取action返回的result
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public string GetResponseValues(HttpActionExecutedContext actionExecutedContext)
        {
            return actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
            //
            /*
            这个StreamReader不能关闭，也不能dispose， 关了就傻逼了
            因为你关掉后，后面的管道  或拦截器就没办法读取了
            */
            //var reader = new StreamReader(stream, encoding);
            //string result = reader.ReadToEnd();
            /*
            这里也要注意：   stream.Position = 0; 
            当你读取完之后必须把stream的位置设为开始
            因为request和response读取完以后Position到最后一个位置，交给下一个方法处理的时候就会读不到内容了。
            */
            //stream.Position = 0;
            //return result;
        }

    }
}