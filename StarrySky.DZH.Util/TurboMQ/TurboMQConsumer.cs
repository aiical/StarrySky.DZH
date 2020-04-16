using System;
using System.Collections.Generic;
using System.Text;
using TCBase.Distributed.TurboMQ.Client.Consumer;
using TCBase.Distributed.TurboMQ.Client.Consumer.Listener;
using TCBase.Distributed.TurboMQ.Common.Consumer;
using TCBase.Distributed.TurboMQ.Common.Message;
using TCBase.Distributed.TurboMQ.Common.Protocol.Heartbeat;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 消费时如果抛了异常，或回了RECONSUME_LATER，之后会延时重试，每次重试时的消息id会变，且取消息的主题时拿到的是%RETRY%开头的字符串。
    /// 所以如果有重试的情况，不要通过消息id是否相同去决定一些业务逻辑。
    /// </summary>
    public class TurboMQConsumer<T> where T : ITruboMQRegister, new()
    {
        private static TurboMQConsumer<T> _instance;

        private TurboMQConsumer()
        {
            if (consumer == null)
            {
                lock (locker)
                {
                    if (consumer == null)
                    {
                        DefaultMQPushConsumer consumer = new DefaultMQPushConsumer(ConfigRegister.Group); //创建consumer，group名称为your_conumer_group, 测试环境请换个自己的组名，直接用例子上的容易和别人撞
                        consumer.NamesrvAddr = TurboMQConfig.NameServer; //设置NameServer地址
                                                                         //设置your_conumer_group第一次上线时，从哪里开始消费消息。例子是从Topic最开始的offset开始读。
                                                                         //注意：设置成CONSUME_FROM_LAST_OFFSET，则第一次上线前的消息都不会消费。这个参数只对该group第一次上线时有效，以后上线时，都会从上次消费的位置开始消费。
                        consumer.ConsumeFromWhere = ConsumeFromWhere.CONSUME_FROM_FIRST_OFFSET;
                        consumer.MessageModel = MessageModel.CLUSTERING; //消费方式，如果设置成MessageModel.CLUSTERING，同一个group中，同一条消息只会被一台机器消费，如果设置成MessageModel.BROADCASTING，同一条消息会被每台机器消费一次。
                        consumer.Subscribe(ConfigRegister.Topic, "*"); //订阅主题是TopicTest的消息，第二个参数为*表示不按Tag过滤消息, 若要订阅多个不同的消息，这里调多次Subscribe就可以了。测试环境请换成自己的Topic，直接用例子上的容易和别人撞。
                        consumer.RegisterMessageListener(new MyMessageListener());//注册消费监听器
                        consumer.Start();//启动消费者
                    }
                }
            }
        }

        private static DefaultMQPushConsumer consumer;
        private static readonly object locker = new object();

        private static T ConfigRegister { get; set; } = new T();
        /// <summary>
        /// 单例并启动
        /// </summary>
        /// <returns></returns>
        public static TurboMQConsumer<T> GetInstanse()
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new TurboMQConsumer<T>();
                    }
                }

            }
            return _instance;
        }

        /// <summary>
        /// 销毁消费者
        /// </summary>
        /// <returns></returns>
        public void Release()
        {
            lock (locker)
            {
                if (consumer != null)
                {
                    consumer.Shutdown();//销毁链接
                    consumer = null;
                }
            }
        }


        private class MyMessageListener : MessageListenerConcurrently //消息监听器
        {
            // 这个方法是会并行执行的，请注意线程安全
            public ConsumeConcurrentlyStatus ConsumeMessage(IList<MessageExt> msgs, ConsumeConcurrentlyContext context) //收到消息时会触发此函数，默认情况下msgs里只会有一条消息。
            {
                var msgId = string.Empty;
                var msgTag = string.Empty;
                var msgBody = string.Empty;
                try
                {
                    foreach (MessageExt msg in msgs)
                    {
                        msgId = msg.MsgId;
                        msgTag = msg.Tags;
                        msgBody = Encoding.UTF8.GetString(msg.Body);
                        //TODO 消费逻辑
                        ConfigRegister.Consumer(msgId, msgBody, msgTag);
                    }
                    return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;//返回CONSUME_SUCCESS表示消费成功，如果返回RECONSUME_LATER，表示消费失败，以后这条消息还会再次消费
                }
                catch (Exception ex)
                {
                    //LogBuilder.Error($"mq消费报错,msgBody={msgBody}", ex, msgTag, msgId);
                    return ConsumeConcurrentlyStatus.RECONSUME_LATER;
                }
            }
        }
    }



}
