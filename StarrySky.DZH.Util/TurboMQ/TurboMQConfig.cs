
using System;
using System.Collections.Generic;
using System.Text;
using TCBase.Distributed.TurboMQ.Client.Producer;
using TCBase.Distributed.TurboMQ.Common.Message;
namespace SurpriseGamePoll.Common.TurboMQ
{

    /// <summary>
    /// 一、消息生产和消费的逻辑如下图，解释如下（如 果用过Kafka等其它消息队列，理解这个逻辑，这部分可跳过不看）：
    ///（1）每条消息由一个生产者生产，它有一个属性，叫“主题”，生产消息时可以指定这个主题。
    ///（2）每个消费者有一个属性，叫“消费者组名”，多个消费者组名相同的消费者构成一个组，每个组可以指定我要订阅哪些主题的消息。
    ///（3）每条消息，会被订阅它的每个组消费一次（发到其中一台消费者上）。
    /// </summary>
    public class TurboMQConfig
    {
        #region config
        /// <summary>
        /// 线下"*****:9876";
        /// </summary>
        public static string NameServer = "*****:9876";

        /// <summary>
        /// The consume message batch maximum size
        /// </summary>
        public int ConsumeMessageBatchMaxSize = 5;

        #endregion
    }


}
