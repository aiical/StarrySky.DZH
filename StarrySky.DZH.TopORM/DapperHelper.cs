using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StarrySky.DZH.TopORM.Common;
using MySql.Data.MySqlClient;

namespace StarrySky.DZH.TopORM
{
    /// <summary>
    /// author:dingzhenhua
    /// conn不带事务时，会自行处理open,用到事务需手动开启
    /// </summary>
    public class DapperHelper
    {
        private static IDbConnection GetDbConnection(string dbName)
        {
            try
            {
                //new SqlConnection(DBConfig.GetDbDataSource(dbName)) //SqlConnection不支持MySQL
                return new MySqlConnection(DBConfig.GetDBConnStr(dbName));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 查询，查不到返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSelectList<T>(string dbName, string sql,  bool isUseRead = false)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            try
            {
                using (var conn = GetDbConnection(dbName))
                {
                    return conn.Query<T>(sql);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 参数化查询，查不到返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param">参数化实体</param>
        /// <returns></returns>
        public static IEnumerable<T> GetSelectListWithParam<T>(string dbName, string sql, object param, bool isUseRead = false)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return null;
            }
            try
            {
                using (var conn = GetDbConnection(dbName))
                {
                    var parameters = new DynamicParameters();
                    conn.Open();
                    return conn.Query<T>(sql, param);
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 批量执行 事务
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="sqls"></param>
        /// <param name="Params"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static int ExecuteBatchWithTransaction(string dbName, List<string> sqls)
        {
            if (sqls == null || !sqls.Any())
            {
                return 0;
            }
            IDbConnection conn = null;
            IDbTransaction transaction = null;
            try
            {
                conn = GetDbConnection(dbName);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                transaction = conn.BeginTransaction();
                StringBuilder sqlBuilder = new StringBuilder();
                foreach (var item in sqls)
                {
                    sqlBuilder.Append(item + ";"); //防止没有传入结尾标记
                }
                var list = conn.Execute(sqlBuilder.ToString(), null, transaction);
                transaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn = null;
                }

            }

        }

        /// <summary>
        /// do not use it to insert if you need return id form the database
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="sql"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public static int Execute(string dbName, string sql, object Params = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            using (var conn = GetDbConnection(dbName))
            {
                var list = conn.Execute(sql, Params, null);
                return list;
            }
        }
        /// <summary>
        /// use it to insert if you need return id form the database
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="sql"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public static int InsertReturnId(string dbName, string sql, object Params)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            using (var conn = GetDbConnection(dbName))
            {
                var list = conn.Execute(sql, Params, null);
                var r = conn.Query("Select LAST_INSERT_ID() id", null);
                return r.FirstOrDefault()?.id ?? 0;
            }
        }
    }



}
