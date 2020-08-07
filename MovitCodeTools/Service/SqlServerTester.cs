using MovitCodeTools.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Service
{
    public class SqlServerTester : IDBTester
    {
        public bool TestConnection(DBConfigModel dbModel)
        {
            var connStr = DBConfigUtil.GenerateSqlServerDBConnectionString(dbModel);
            DbConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
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

        public bool TestConnection(string connStr)
        {
            DbConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
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