using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Common
{
    public static class DBConfig
    {
        private static string mysqlconnStr = "User ID=root;Password=123456;DataBase=mysql;Server=localhost;Port=3306;Min Pool Size=1;Max Pool Size=100;CharSet=utf8;SslMode=none;";

        private static string _dbName;

        public static string GetDBConnStr(string dbName)
        {
            return mysqlconnStr;
        }
    }
}
