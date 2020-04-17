using System;
using System.Collections.Generic;
using System.Text;
using TCBase.Distributed.TurboMQ.Client.Producer;
using TCBase.Distributed.TurboMQ.Common.Message;

namespace SurpriseGamePoll.Common.TurboMQ
{
    /// <summary>
    /// 注意，生产者的创建、启动是很重的操作，
    /// 实际使用时，请不要频繁地创建生产者。
    /// 建议一个程序中，相同的group name只使用一个生产者。
    /// 并发地调用生产者对象的发送方法是线程安全的。
    /// </summary>
    public class TurboMQProvider<T> where T : ITruboMQRegister, new()
    {
        /// <summary>
        /// 初始化 生产者
        /// </summary>
        private TurboMQProvider()
        {
            //创建生产者，my_group_name是用来标识消息生产者的组名，生产者的组名只用于在后台区分实例是哪个应用启的，没有其它作用。
            producer = new DefaultMQProducer(ConfigRegister.Group);
            //设置RocketMQ NameServer的地址
            producer.NamesrvAddr = TurboMQConfig.NameServer;
            //同步发送时,若后台一组broker 响应慢或崩溃, 生产者会尝试其他的broker
            producer.RetryAnotherBrokerWhenNotStoreOK = true;
            producer.Start(); //启动生产者
        }

        /// <summary>
        /// 安全锁
        /// </summary>
        private static readonly object locker = new object();

        private static TurboMQProvider<T> _instance;

        private static DefaultMQProducer producer; //同一个group的生产者一般尽量只创建一个，每次发消息时重复使用。

        private static T ConfigRegister { get; set; } = new T();
        /// <summary>
        /// 单例并启动
        /// </summary>
        /// <returns></returns>
        public static TurboMQProvider<T> GetInstance()
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new TurboMQProvider<T>();
                    }
                }

            }
            return _instance;
        }


        #region 功能代码
        /// <summary>
        /// 
        /// </summary>
        public bool SendMQMsg<U>(MqSendModel<U> model, int delayTimeLevel = 0)
        {
            
            try
            {
                // tag，消费方可以根据这个tag在接收时过滤消息。
                Message msg = new Message(ConfigRegister.Topic, model.Tag, model.BTbody);
                if (delayTimeLevel > 0)
                {
                    msg.DelayTimeLevel = delayTimeLevel;
                }
                SendResult sendResult = producer.Send(msg); //发送消息，这是个同步方法，在发送成功或失败前会阻塞。发送失败会抛出MQClientException，RemotingException或MQBrokerException
                SendStatus status = sendResult.GetSendStatus();
                return status == SendStatus.SEND_OK;
            }
            catch (Exception ex)
            {
                //LogBuilder.Error($"{model?.PackJson()}", ex, "SendMQMsg", model.Tag);
                return false;
            }
            finally
            {
                //LogBuilder.Info($"{ConfigRegister.Topic},{ConfigRegister.Group},{model?.PackJson()}", "SendMQMsg", model.Tag);
            }
        }


        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            if (producer != null)
            {
                lock (locker)
                {
                    if (producer != null)
                    {
                        //如果确实要多次创建producer，每次使用完一定要调用Shutdown销毁连接。
                        producer.Shutdown();
                    }
                }
            }
        }
        #endregion




    }

}
