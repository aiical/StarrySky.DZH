using SurpriseGamePoll.Common.TurboMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurpriseGamePoll.Services.TurboMQBusiness
{
    /// <summary>
    /// gamebase主题mq注册
    /// ☆☆☆☆此主题下的消费者禁止使用，因为game站开启了
    /// </summary>
    [Obsolete("此主题下的消费者禁止使用，因为game站开启了")]
    public class SurpriseMQRegister : ITruboMQRegister
    {
        public string Topic
        {
            get
            {
                return "qyxm_qyrjkf_topic_gamebase";
            }
            set { }
        }

        public string Group
        {
            get
            {
                return "qyxm_qyrjkf_group_gamebase";
            }

            set { }
        }


        #region 定制化配置参数
        /// <summary>资金流水新增mq</summary>
        public const string Tags_AddBillRecord = "tags_addbillrecord";

        /// <summary>资金流水完结</summary>
        public const string Tags_FinishBill = "tags_finishbill";

        /// <summary>红包中资金流水mq</summary>
        public const string Tags_SendMoneyAddBillRecord = "tags_sendmoneyaddbillrecord";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgbody"></param>
        /// <param name="tag"></param>
        [Obsolete("此主题下的消费者禁止使用，因为game站开启了")]
        public void Consumer(string msgId, string msgbody, string tag)
        {
            switch (tag.ToLower())
            {
                case Tags_AddBillRecord:
                    //new MoneyFlowBillService().AddBillRecordMQConsume(message);
                    break;
                case Tags_SendMoneyAddBillRecord:
                    //new MoneyFlowBillService().UpdateBillMQConsumer(message);
                    break;
                case Tags_FinishBill:
                    //new MoneyFlowBillService().FinishOrderBillConsumer(message);
                    break;
            }
        }
    }
}
