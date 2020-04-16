
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 
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
        /// byte字节
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
