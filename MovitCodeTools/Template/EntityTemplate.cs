using MovitCodeTools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovitCodeTools.Template
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
        public static string GetTemplateString(string tableName, List<TableDetailsInfo> tables, string projectName = "ORMCore")
        {
            if (tables == null)
            {
                return "error:没有查询到表的字段数据";
            }
            var primaryKey = string.Empty;
            var modelStr = new StringBuilder();
            foreach (var item in tables)
            {
                var fieldKeyStr = string.Empty;
                var colType = string.Empty;
                if (item.CollIsPrimary == 1)
                {
                    fieldKeyStr = "\r\n        [EntityColumn(IsPrimaryKey = true)]";
                }
                colType = ORMUtil.GetSystemTypeString(item.CollType);
                var description = string.IsNullOrWhiteSpace(item.CollDescription) ? item.CollName : item.CollDescription;
                modelStr.Append($@"
        /// <summary>
        /// {description}
        /// </summary>{fieldKeyStr}
        public {colType} {item.CollName} {{ get; set; }}");

            }


            var tempStr = new StringBuilder();
            tempStr.Append($@"using System;
using MT.Enterprise.Core.ORM;

namespace CCM.Console.Core.Entity
{{ 
    /// <summary>
    /// {tableName}表信息
    /// </summary>
    [EntityTable(""{ tableName}"")]
    public class {tableName.Replace("_", "")}
    {{ 
       {modelStr}
    }}
}}");

            return tempStr.ToString();
        }

    }
}
