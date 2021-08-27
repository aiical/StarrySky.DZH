using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StarrySky.DZH.WebDemo.Models;

namespace StarrySky.DZH.WebDemo.CustomAttribute
{
    /// <summary>
    /// 过滤器 // 不能 using System.Web.Http.Filters; 要using Microsoft.AspNetCore和using Microsoft.AspNetCore.Mvc; 否则没有OnResultExecuting和OnResultExecuted
    /// </summary>
    public class BPMFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 表单主键
        /// </summary>
        private string FormId = null;

        /// <summary>
        /// 数据请求版本标识
        /// </summary>
        private string DataVersionGuid = null;

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="context">上下文</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.ActionArguments.Values.FirstOrDefault() as BaseRequest;
            FormId = request.FormID;
            request.DataVersionGuid = Guid.NewGuid().ToString().Replace("-", "");
            DataVersionGuid = request.DataVersionGuid;
            // throw new StatusBadRequestException("已提交过bpm申请，请直接提交!");
            // context.Result = new EmptyResult();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // 如果action中抛出异常，这里会收到context.Exception，且不会走OnResultExecuting和OnResultExecuted，直接api结束。
            // 否则context.Exception为null,会进入走OnResultExecuting和OnResultExecuted
            bool isError = context.Exception != null;
            if (!isError)
            {
                // TODO:写入数据版本到数据库中,便于bpmmq回调是通过此值判断数据是否一致

            }
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="context">上下文</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // var relate = ServiceHelperProvider.Instance.BpmRelative.GetBPMCCMByFormID(FormId);
            // var ulr = MTAPIService.GetBPMStartUrl(FormId, relate?.OACode, relate?.CreateUserID, null);
            PathString path = context.HttpContext.Request.Path;
            if (!path.HasValue || context.Result is FileResult)
            {
                return;
            }

            if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { StartUrl = "" });
            }
        }

        /// <summary>
        /// OnResultExecuted
        /// </summary>
        /// <param name="context">context</param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}