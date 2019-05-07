using GenerateToolApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateToolApp.Template
{
    public class EntityTemplate
    {
        /// <summary>
        /// 获取数据实体模版字符
        /// </summary>
        /// <param name="databaseName">数据库名</param>
        /// <param name="tableName">表名</param>
        /// <param name="tables">表信息</param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static string GetTemplateString(string databaseName, string tableName, dynamic tables, string projectName = "ORMCore")
        {
            if (tables == null)
            {
                return "error:没有查询到表的字段数据";
            }
            var primaryKey = string.Empty;
            var modelStr = new StringBuilder();
            var fieldStr = new StringBuilder();
            foreach (var item in tables)
            {
                var modelKeyStr = string.Empty;
                var fieldKeyStr = string.Empty;
                var fieldCnKeyStr = string.Empty;
                if (item.COLUMN_KEY == "PRI")
                {
                    primaryKey = item.COLUMN_NAME;
                    modelKeyStr = "\r\n        [Key]";
                    fieldKeyStr = "\r\n        [PrimaryKey]";
                    fieldCnKeyStr = "[主键]";
                }

                string dataType = ORMUtil.GetSystemTypeString(item.DATA_TYPE);
                string defaultValue = ORMUtil.GetDefaultValueString(item.DATA_TYPE, item.COLUMN_DEFAULT);
                defaultValue = !string.IsNullOrEmpty(defaultValue) ? defaultValue + ";" : "";
                var timeType = new List<string> { "timestamp", "datetime", "smalldatetime", "time" };
                var timeCvt =
                    string.Empty; //timeType.Contains(item.DATA_TYPE.ToLower()) ? ".PackDateTime().ToString(\"yyyy-MM-dd HH:mm:ss\")" : "";
                modelStr.Append($@"
        /// <summary>
        /// {item.COLUMN_COMMENT}
        /// </summary>{modelKeyStr}
        public {dataType} {item.COLUMN_NAME}{{ get;set; }}{defaultValue}");


                fieldStr.Append($@"
        /// <summary>
        /// {item.COLUMN_COMMENT}{fieldCnKeyStr}
        /// </summary>
        [Description(""{item.COLUMN_COMMENT}"")]{fieldKeyStr}
        {item.COLUMN_NAME},");
            }


            var tempStr = new StringBuilder();
            tempStr.Append($@"using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel; 
using ORM;
using Utility;

namespace {projectName}.Entity
{{ 
    /// <summary>
    /// {tableName}表信息
    /// </summary>
    public class {tableName}TableInfo
    {{ 
        /// <summary>
        /// 初始化
        /// </summary>
        static {tableName}TableInfo() 
        {{
            tableName = ""{tableName}"";
            primaryKey = ""{primaryKey}"";
        }}

        /// <summary>
        /// 表名称
        /// </summary>
        public static string tableName {{ get; set; }}

        /// <summary>
        /// 表主键
        /// </summary>
        public static string primaryKey {{ get; set; }}
    }}

    /// <summary>
    /// {tableName}表实体类
    /// </summary>
    [Serializable, DataBase(""{databaseName}""), Table(""{tableName}""), DataBaseConfig(""MySQL"")]
    public class {tableName}Entity
    {{
        {modelStr}
    }}

    /// <summary>
    /// {tableName}表字段
    /// </summary>
    [TableInfo(""{databaseName}"", ""{tableName}"", ""MySQL"")]
    public enum {tableName}Fields
    {{{fieldStr}
    }}
}}");
            return tempStr.ToString();
        }

    }
}
