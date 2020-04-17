using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 发送的mq消息DataTransferObject对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MqSendModel<T>
    {
        /// <summary>
        /// tag标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 消息发送主体
        /// </summary>
        public T Msg { get; set; }

        /// <summary>
        /// 转化的byte字节
        /// </summary>
        public byte[] BTbody
        {
            get
            {
                return Encoding.UTF8.GetBytes(Msg?.PackJson());
            }
        }
    }
}
