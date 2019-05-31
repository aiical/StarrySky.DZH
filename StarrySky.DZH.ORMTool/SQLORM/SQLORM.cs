using StarrySky.DZH.ORMTool.SQLORM.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM
{
    public class SqlORM<T> where T : class
    {
        /// <summary>
        /// 新增一个实体并返回主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int AddEntityShowId(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.InsertReturnId("dzhMySQL", sql, model);
            var type = typeof(T);
            type.GetProperty(primaryProp.Name).SetValue(model, id);
            return id;
        }

        public static bool AddEntity(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        public static  bool UpdateEntityById(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToUpdateSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        #region lambda 链式更新
        public static SqlORM<T> UpdateCustom(Expression<Func<T, bool>> exp)
        {
            var Body = exp.Body;
            var Name = exp.Name;
            var NodeType = exp.NodeType;
            var Parameters = exp.Parameters;
            var ReturnType = exp.ReturnType;
            var type = exp.Type;

            //return ;
        }
        #endregion

    }
}
