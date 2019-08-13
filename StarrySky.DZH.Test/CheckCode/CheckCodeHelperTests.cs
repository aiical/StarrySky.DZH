using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.Util.CheckCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.CheckCode.Tests
{
    [TestClass()]
    public class CheckCodeHelperTests
    {
        [TestMethod()]
        public void CreateCheckCodeImageTest()
        {
            var code=CheckCodeHelper.GenerateCode(4);
            CheckCodeHelper.CreateCheckCodeImage(code);
        }
    }
}