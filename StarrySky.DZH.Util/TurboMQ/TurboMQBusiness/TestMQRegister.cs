using SurpriseGamePoll.Common.TurboMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Services.TurboMQBusiness
{
    /// <summary>
    /// 测试主题mq注册
    /// qyxm_qyrjkf_topic_spmrecord 为波波申请的正式主题 但是一直没有使用，
    /// 建议重新申请
    /// </summary>
    public class TestMQRegister : ITruboMQRegister
    {
        public string Topic
        {
            get
            {
                return "qyxm_qyrjkf_topic_spmrecord";
            }
            set { }
        }

        public string Group
        {
            get
            {
                return "qyxm_qyrjkf_group_spmrecord";
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
        /// 要实现的消费逻辑 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgbody"></param>
        /// <param name="tag">自行定义扩展，并实现对应的处理方法</param>
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