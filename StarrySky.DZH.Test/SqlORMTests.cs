using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.TopORM;
using StarrySky.DZH.TopORM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Tests
{
    [TestClass()]
    public class SqlORMTests
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

            sqlORM.UpdateCustom(p => p.DId == 1).ExcuteNonQuery();
            sqlORM.UpdateCustom(p => new DemoEntity() { DName = "horse", DAddress = "32" }).ExcuteNonQuery();//MemberInitExpression

        }


        [TestMethod()]
        public void ToSelectTest()
        {

            //var model1 = sqlORM.Select(t => t.DId);  //MemberExpression
            //var model2 = sqlORM.Select(t => t);  //ParameterExpression
            //var model3 = sqlORM.Select(t => new { t.DId, t.DName }); //NewExpression
            var model4 = sqlORM.Where(t => t.DId == 1).Select(t => t.DId);

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