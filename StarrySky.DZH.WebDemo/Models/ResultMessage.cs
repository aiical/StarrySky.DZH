using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarrySky.DZH.WebDemo.Models
{
    public class ResultMessage<T>
    {
        private ResultMessage() { }

        public ResultMessage(int code, string msg, T body)
        {
            ResultMsg = msg;
            ResultCode = code;
            Body = body;
        }

        public string ResultMsg { get; set; }
        /// <summary>
        /// 返回编码1成功2失败
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        ///  响应信息
        /// </summary>
        public T Body { get; set; }
    }
}