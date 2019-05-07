using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.Util.DataConvert;

namespace StarrySky.DZH.Test.DataConvert
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string hanzi = "中国，水电焊加工“ 哈哈'sdfh 是我";
            var s = PinYinConvertHelper.GetFirstPinyin(hanzi);
            var s2 = PinYinConvertHelper.GetPinyin(hanzi);
        }
    }
}
