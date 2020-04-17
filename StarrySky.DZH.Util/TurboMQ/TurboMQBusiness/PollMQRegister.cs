using SurpriseGamePoll.Common.TurboMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Services.TurboMQBusiness
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
                return "qyxm_qyrjkf_topic_gamepoll";
            }
            set { }
        }

        public string Group
        {
            get
            {
                return "qyxm_qyrjkf_group_gamepoll";
            }

            set { }
        }


        #region 定制化配置参数
        /// <summary>
        /// 测试用tags
        /// </summary>
        public const string Tag_Test = "tags_test";
        /// <summary>
        /// 任务中心
        /// </summary>
        public const string Tag_UpdateMission = "tags_updatemission";

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
                case Tag_UpdateMission:
                    //new MissionCenterService().UpsetMissionRecordMQConsumer(msgbody);
                    break;
            }
        }
    }
}
