using SurpriseGamePoll.Common.TurboMQ;
using SurpriseGamePoll.Services.TurboMQBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.TurboMQ.MQUnitTest
{
    public class MQTest
    {
        public string MQProduceTest(string postkey)
        {
            var re = new BaseRequest()
            {
                PostKey = postkey,
            };
            TurboMQProvider<PollMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = PollMQRegister.Tag_Test,
                Msg = re
            });
            TurboMQProvider<TestMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = TestMQRegister.Tag_Test,
                Msg = re
            });
            TurboMQProvider<PollMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = PollMQRegister.Tag_Test,
                Msg = re
            });
            TurboMQProvider<TestMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = TestMQRegister.Tag_Test,
                Msg = re
            });
            TurboMQProvider<PollMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = PollMQRegister.Tag_Test,
                Msg = re
            });
            TurboMQProvider<TestMQRegister>.GetInstance().SendMQMsg<BaseRequest>(new MqSendModel<BaseRequest>()
            {
                Tag = TestMQRegister.Tag_Test,
                Msg = re
            });
            return "";
        }
    }

    internal class BaseRequest
    {
        public BaseRequest()
        {
        }

        public string PostKey { get; set; }
    }
}
