using SurpriseGamePoll.Common.TurboMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Services.TurboMQ
{
    /// <summary>
    /// poll主题mq注册
    /// </summary>
    public class PollMQRegister : ITruboMQRegister
    {
        public string Topic
        {
            get
            {
                return "****_topic_gamepoll";
            }
            set { }
        }

        public string Group
        {
            get
            {
                return "****_group_gamepoll";
            }

            set { }
        }


        #region 定制化配置参数
        /// <summary>
        /// 测试用tags
        /// </summary>
        public const string Tag_Test = "tags_test";

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgbody"></param>
        /// <param name="tag"></param>
        public void Consumer(string msgId, string msgbody, string tag)
        {
            switch (tag.ToLower())
            {
                case Tag_Test:
                    new TestDemo().TestTag(msgbody);
                    break;
               
            }
        }
    }
}
