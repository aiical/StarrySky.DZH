using StarrySky.DZH.ORMTool.SQLORM.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM
{
    public class SqlORM
    {
        /// <summary>
        /// 新增一个实体并返回主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddEntityShowId<T>(T model) where T : class
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.InsertReturnId("dzhMySQL", sql, model);
            var type = typeof(T);
            type.GetProperty(primaryProp.Name).SetValue(model, id);
            return id;
        }

        public bool AddEntity<T>(T model) where T : class
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        public bool UpdateEntityById<T>(T model) where T : class
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToUpdateSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }
    }
}
