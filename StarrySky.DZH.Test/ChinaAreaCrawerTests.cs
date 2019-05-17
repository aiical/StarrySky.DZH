using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Tests
{
    [TestClass()]
    public class ChinaAreaCrawerTests
    {
        [TestMethod()]
        public void CreateJsonTest()
        {
            ChinaAreaCrawer.CreateJson();
            Assert.Fail();
        }
    }
}