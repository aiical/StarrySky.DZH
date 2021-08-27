using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Common
{
    public class MySqlDBA
    {
        /// <summary>
        /// 查询db下所有表名和表注释
        /// </summary>
        /// <param name="connStr">连接字串</param>
        /// <returns></returns>
        public static string AllTablesSql(string connStr) =>
            $@"select table_name tableName, 
                      table_comment tableComment,
                      create_time createTime 
               from information_schema.tables 
               where table_schema = '{connStr.Split(new[] { "DataBase=", "Database=" },
                 StringSplitOptions.RemoveEmptyEntries)[1].Split(';')[0]}'";

        /// <summary>
        /// 查表下所有列的所有信息（参数化sql）
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static string AllColumnsSql() =>
           $@" SELECT 
                    * 
               FROM INFORMATION_SCHEMA.Columns 
               WHERE table_name=@tableName  AND table_schema=@dbName";
    }
}
