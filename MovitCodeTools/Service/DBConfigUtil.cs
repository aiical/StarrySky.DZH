using MovitCodeTools.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Service
{
    public class DBConfigUtil
    {
        /// <summary>
        /// 生成数据库连接字串
        /// </summary>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        public static string GenerateDBConnectionString(DBConfigModel dbModel) {
            if (dbModel == null) {
                return "";
            }
            return $"User ID={dbModel.UserName};Password={dbModel.Password};DataBase={dbModel.DBName};Server={dbModel.IP};Port={dbModel.Port};Min Pool Size=1;Max Pool Size=100;CharSet=utf8;SslMode=none;";
        }

        /// <summary>
        /// 生成数据库连接字串
        /// </summary>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        public static string GenerateSqlServerDBConnectionString(DBConfigModel dbModel)
        {
            if (dbModel == null)
            {
                return "";
            }
            return  $"Server={dbModel.IP};Database={dbModel.DBName};Integrated Security=False;User ID={dbModel.UserName};Password={dbModel.Password}";
        }


    }
}
