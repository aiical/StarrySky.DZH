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
        #region insert
        /// <summary>
        /// 新增一个实体并返回主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddEntityShowId(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.InsertReturnId("dzhMySQL", sql, model);
            var type = typeof(T);
            type.GetProperty(primaryProp.Name).SetValue(model, id);
            return id;
        }

        public bool AddEntity(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToInsertSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        #endregion

        #region update
        public bool UpdateEntityById(T model)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToUpdateSql(model, out primaryProp);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        #region lambda 链式更新
        public SqlORM<T> UpdateCustom<V>(Expression<Func<T, V>> exp)
        {
            var Body = exp.Body;
            var Name = exp.Name;
            var NodeType = exp.NodeType;
            var Parameters = exp.Parameters;
            var ReturnType = exp.ReturnType;
            var type = exp.Type;

            return this;
        }
        #endregion

        //按条件部分更新
        public virtual int Update(Expression<Func<T>> update, Expression<Func<T, bool>> where)
        {
            return 0;
        }
        #endregion

        #region select

        public SqlORM<T> Select<V>(Expression<Func<T, V>> selectCol)
        {

            return this;
        }
        public SqlORM<T> SelectIngore<V>(Expression<Func<T, V>> ingoreCol)
        {
            return this;
        }

        public SqlORM<T> Where(Expression<Func<T, bool>> where)
        {
            return this;
        }
        public T Excute()
        {

            return default(T);
        }
        #endregion

        #region delete
        /// <summary>
        /// deleted by primarykey
        /// 数据无价，硬删除，谨慎使用!
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        [Obsolete("数据无价，硬删除，谨慎使用!")]
        public bool DeleteById(int id)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToDeleteSql(default(T), out primaryProp);
            //KeyValuePair<string, int> keyVal = new KeyValuePair<string, int>(primaryProp.Name, id);
            var result = DapperHelper.Execute("dzhMySQL", sql, new { Key = id});
            return result > 1;
        }
        #endregion

        #region invalid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Invalid(Expression<Func<T,bool>> predicate)
        {
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToDeleteSql(default(T), out primaryProp);
            //KeyValuePair<string, int> keyVal = new KeyValuePair<string, int>(primaryProp.Name, id);
            var result = DapperHelper.Execute("dzhMySQL", sql, new { Key = id });
            return result > 1;
        }
        #endregion
    }
}
