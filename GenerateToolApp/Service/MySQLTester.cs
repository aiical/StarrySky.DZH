using GenerateToolApp.Model;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace GenerateToolApp.Service
{
    /// <summary>
    /// 需要nuget引用MySql.Data
    /// </summary>
    public class MySQLTester : IDBTester
    {
        public bool TestConnection(DatabaseModel dbModel)
        {
            var connStr = DatabaseConfigUtil.GenerateDBConnectionString(dbModel);
            DbConnection conn = null;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
            }
        }
    }
}
