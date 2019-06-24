using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.ORMTool.SQLORM;
using StarrySky.DZH.ORMTool.SQLORM.Entity;
using StarrySky.DZH.ORMTool.SQLORM.ExpressionLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM.Tests
{
    [TestClass()]
    public class SqlBuilderTests
    {
        SqlORM<DemoEntity> sqlORM = new SqlORM<DemoEntity>();
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
                DAddress = "3"
            };
            sqlORM.AddEntityShowId(model);
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
            //sqlORM.UpdateEntityById(model);

            sqlORM.UpdateCustom(p => p.DId == 1).Excute();
        }


        [TestMethod()]
        public void ToSelectTest()
        {
            var model1 = sqlORM.Select(t => t.DId).Excute();
            var model2 = sqlORM.Select(t => t).Excute();
            var model3 = sqlORM.Select(t => new { t.DId, t.DName }).Excute();
            //var model1 = sqlORM.Select(t => t.DId).Where(t => t.DId == 1).Excute();

        }

        [TestMethod()]
        public void DeleteByIdTest()
        {
            sqlORM.DeleteById(4);
        }

        [TestMethod()]
        public void InvalidTest()
        {
            sqlORM.Invalid(p => p.DId == 3);
        }
    }
}