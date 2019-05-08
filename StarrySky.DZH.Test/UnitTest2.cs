using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.RabbitMQ;
using StarrySky.DZH.Util.Redis;

namespace StarrySky.DZH.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            //RedisHelper.Set<string>("ding", 20, "你试试");
            //var ss= RedisHelper.Get<string>("ding");

            HelloWord.HelloWordClient();
        }
    }
}
