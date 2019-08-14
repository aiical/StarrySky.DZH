﻿using Dapper;
using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.CustomAttribute;
using StarrySky.DZH.TopORM.ExpressionLib;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM
{
    public class TopORM<T> where T : class
    {
        private DBOperateStatusEnum operateStatus = DBOperateStatusEnum.Default;
        private List<string> selectColumn = null;
        private List<string> updateColumn = null;
        private List<string> whereColumn = null;
        //声明动态参数
        private DynamicParameters updateParameters = null;
        private DynamicParameters whereParameters = null;

        public TopORM()
        {
            selectColumn = new List<string>();
            updateColumn = new List<string>();
            whereColumn = new List<string>();
            updateParameters = new DynamicParameters();
            whereParameters = new DynamicParameters();
        }

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
            var sql = SqlBuilder.ToUpdateSql(model);
            var id = DapperHelper.Execute("dzhMySQL", sql, model);
            return id > 0;
        }

        #region lambda 链式更新
        public TopORM<T> UpdateCustom<V>(Expression<Func<T, V>> exp)
        {
            operateStatus = DBOperateStatusEnum.Edit;
            updateColumn = ExpressionTranslator.GetUpdateColumn(exp, updateParameters);
            return this;
        }
        #endregion

        #endregion

        #region select

        public IEnumerable<V> Select<V>(Expression<Func<T, V>> selectCol)
        {
            operateStatus = DBOperateStatusEnum.Query;
            selectColumn = ExpressionTranslator.GetSelectColumn(selectCol);
            return DapperHelper.GetSelectListWithParam<V>("dzhMySQL", GetQuerySql(), whereParameters);
        }

        private string GetQuerySql()
        {
            Type type = typeof(T);
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());
            string selectField = selectColumn == null ? "*" : string.Join(",", selectColumn);
            string whereField = whereColumn == null ? "" : string.Join("and", whereColumn);
            return $@"SELECT {selectField}  FROM `{tableInfo.TableName}` WHERE 1=1 {whereField}";
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
            var result = DapperHelper.Execute("dzhMySQL", sql, new { Key = id });
            return result > 1;
        }
        #endregion

        #region invalid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Invalid(Expression<Func<T, bool>> predicate)
        {
            operateStatus = DBOperateStatusEnum.Invalid;
            PropertyInfo primaryProp;
            var sql = SqlBuilder.ToDeleteSql(default(T), out primaryProp);
            //KeyValuePair<string, int> keyVal = new KeyValuePair<string, int>(primaryProp.Name, id);
            var result = DapperHelper.Execute("dzhMySQL", sql, null);
            return result > 1;
        }
        #endregion

        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TopORM<T> Where(Expression<Func<T, bool>> where)
        {
            whereColumn = ExpressionTranslator.GetWhereColumn(where, whereParameters);
            return this;
        }

        public int ExcuteNonQuery()
        {
            if (operateStatus != DBOperateStatusEnum.Edit)
            {
                throw new NotSupportedException("不支持");
            }
            var parameters = updateParameters;
            parameters.AddDynamicParams(whereParameters);
            return DapperHelper.Execute("dzhMySQL", GetUpdateSql(), parameters);
        }

        private string GetUpdateSql()
        {
            if (updateColumn.IsNullOrEmptyCollection())
            {
                return "";
            }
            Type type = typeof(T);
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());
            string updateField = string.Join(",", updateColumn);
            string whereField = whereColumn == null ? "" : $"and 1=1";
            return $@"UPDATE `{tableInfo.TableName}` SET {updateField} WHERE 1=1 {whereField}";
        }
    }
}