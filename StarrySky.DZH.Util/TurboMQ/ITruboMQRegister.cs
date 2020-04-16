using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITruboMQRegister
    {
        /// <summary>
        /// 消息主题
        /// </summary>
        string Topic { get; set; }
        /// <summary>
        /// 消息组别
        /// </summary>
        string Group { get; set; }
        /// <summary>
        /// 消费逻辑实现方法
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgbody"></param>
        /// <param name="tag"></param>
        void Consumer(string msgId,string msgbody, string tag);

    }
}
