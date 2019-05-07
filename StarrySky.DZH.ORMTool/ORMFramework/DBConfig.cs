using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.ORMFramework
{
    public class DBConfig
    {
        public const string MyDbName = "MyDbName";

        public static string GetDbDataSource(string dbName)
        {
            string source = null;
            switch (dbName)
            {
                case MyDbName:
                    source = ""; break;
                default: break;
            }
            return source;
        }

        public static IDbConnection GetDbConnection(string dataSource)
        {
            return new MySqlConnection(dataSource);
        }
        //
        // 摘要:
        //     为Dapper.Contrib提供数据库类型适配 SqlMapperExtensions.GetDatabaseType = DataSource.GetDatabaseType
        //
        // 参数:
        //   connection:
        public static string GetDatabaseType(IDbConnection connection)
        {
            return "MySQL";
        }
    }
}
