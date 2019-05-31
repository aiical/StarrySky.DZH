using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.ORMTool.SQLORM;
using StarrySky.DZH.ORMTool.SQLORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.Tests
{
    [TestClass()]
    public class SqlBuilderTests
    {
        [TestMethod()]
        public void ToReflectTest()
        {
            var model = new DemoEntity()
            {
                DId = 34,
                DName = "zolo",
                DSex = 18,
                DAddress = "3"
            };
             SqlBuilder.TestReflect(model);
        }


        [TestMethod()]
        public void ToInsertSqlTest()
        {
            var model = new DemoEntity()
            {
                DId = 34,
                DName = "zolo",
                DSex = 18,
                DAddress="3"
            };
            //new SqlORM().AddEntityShowId(model);
        }

        [TestMethod()]
        public void ToUpdateSqlTest()
        {
            var model = new DemoEntity()
            {
                DId = 1,
                DName = "luffy",
                DSex = 222,
                DAddress = "3"
            };
            new SqlORM().UpdateEntityById(model);
        }
    }
}