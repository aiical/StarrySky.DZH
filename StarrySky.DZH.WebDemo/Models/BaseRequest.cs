using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarrySky.DZH.WebDemo.Models
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// 表单主键
        /// </summary>
        public abstract string FormID { get; }

        /// <summary>
        /// 提交数据接口的唯一标识，每次请求都是一个新的
        /// </summary>
        public string DataVersionGuid { get; set; }
    }
}