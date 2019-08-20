using Microsoft.VisualStudio.TestTools.UnitTesting;
using StarrySky.DZH.TopORM;
using StarrySky.DZH.TopORM.Entity;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Tests
{
    [TestClass()]
    public class TopORMTests
    {
        TopORM<DemoEntity> sqlORM = new TopORM<DemoEntity>();
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
                DAddress = "3",
                DUpdateTime = "2018-01-01".PackDateTime().AddDays(1)
            };
            #region 测试通过
            //sqlORM.UpdateEntityById(model);
            //sqlORM.UpdateCustom(p => new DemoEntity() { DSex = 111, DRowStatus = 22 }).ExcuteNonQuery();//MemberInitExpression
            //var res=sqlORM.UpdateCustom(p => p.DRowStatus == 1).ExcuteNonQuery();
            //sqlORM.UpdateCustom(p => p.DRowStatus == 2 && p.DCreateUser == "dzh" && p.DUpdateTime == model.DUpdateTime).ExcuteNonQuery();
            #endregion
            var res=sqlORM.UpdateCustom(p => p.DAddress == "测试1").Where(p=>p.DRowStatus==1&&p.DSex!=21).ExcuteNonQuery();




        }


        [TestMethod()]
        public void ToSelectTest()
        {
            try
            {
                #region 测试通过
                //var model1 = sqlORM.Select(t => t.DId);  //MemberExpression
                //var model2 = sqlORM.Select(t => t);  //ParameterExpression
                //var model3 = sqlORM.Select(t => new { t.DId, t.DName }); //NewExpression
                //var model5 = sqlORM.Where(t => t.DId == 1 && t.DSex == 22).Select(t => t.DId);
                //var model4 = sqlORM.Where(t => t.DId == 1 || t.DSex == 22).Select(t => t);
                //var model11 = sqlORM.Where(t => t.DId == 1 && (t.DRowStatus == 2 || t.DSex == 22)).Select(t => t);
                //var model12 = sqlORM.Where(t => t.DId == 1 || (t.DRowStatus == 2 && t.DSex == 22)).Select(t => t);
                #endregion


                //var model6 = sqlORM.Where(t => t.DName.Substring(0, 2) == "aa").Select(t => t.DId);
                //var model7 = sqlORM.Where(t => t.DName.Contains("aa")).Select(t => t.DId);
                //var model10 = sqlORM.Where(t => t.DId == 1 && (t.DRowStatus == 2 || t.DSex == 22) && !(t.DRowStatus == 3)).Select(t => t.DId); //不支持一元表达式

                var model13 = sqlORM.Where(t => t.DId == 1 && (t.DRowStatus == 2 || t.DSex == 22) && (t.DRowStatus != 3)).Select(t => t.DId);
            }
            catch (Exception ex) {

            }

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