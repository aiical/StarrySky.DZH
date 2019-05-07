using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StarrySky.DZH.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            decimal min = 10.01M;
            decimal max = 200.03m;
            decimal result;
            int seek = DateTime.Now.Millisecond;
            Random random = new Random(seek);
            if (max - min >= 1)
            {
                //精确小数后一位 （64.01， 65.01；可能random出64） 
                result = random.Next(Convert.ToInt32(min * 10), Convert.ToInt32(max * 10)) / 10m;
                if (result <= min || result >= max)
                {
                    result = (min + max) / 2;
                }
            }
            else
            {
                //精确小数两位
                result = random.Next(Convert.ToInt32(min * 100), Convert.ToInt32(max * 100)) / 100m;
                if (result <= min || result >= max)
                {
                    result = (min + max) / 2;
                }
            }
            var ss= Convert.ToDecimal(Math.Floor(result * 100m) / 100m);

        }
    }
}
