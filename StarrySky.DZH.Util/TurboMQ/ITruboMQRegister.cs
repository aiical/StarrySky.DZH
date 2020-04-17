using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 由业务实现
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
        /// 要实现的消费逻辑 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgbody"></param>
        /// <param name="tag">自行定义扩展，并实现对应的处理方法</param>
        void Consumer(string msgId,string msgbody, string tag);

    }
}
