using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.Util.EnumUtil;

namespace StarrySky.DZH.Test
{
    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string result = DriveType.Bus.GetEnumDescription();
            string result2 = ((DriveType)2).GetEnumDescription();
            var  t = EnumHelper.TryParseToEnum<DriveType>("Bus",out var e);
            var  t2 = EnumHelper.TryParseToEnum<DriveType>("Bussss", out var e2);
            var s=EnumHelper.GetEnum<DriveType>("公交");
            Console.WriteLine("EnumDesc:" + result);
        }
    }


    public enum DriveType
    {
        
        /// <summary>
        /// 公交车
        /// </summary>
        [System.ComponentModel.Description("公交")]
        Bus = 1,
        /// <summary>
        /// 开车
        /// </summary>
        [System.ComponentModel.Description("汽车")]
        Car = 2,
        /// <summary>
        /// 步行
        /// </summary>
        [System.ComponentModel.Description("步行")]
        Run = 3

    }
}
