using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Common.Tests
{
    [TestClass()]
    public class GenericPropMappingTests
    {
        [TestMethod()]
        public void GetPropTest()
        {
            DemoEntity demo = new DemoEntity();
            var s=GenericMappingProvider.GetClassConstruction<DemoEntity>(demo);
        }
    }
}