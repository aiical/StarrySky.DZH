using StarrySky.DZH.Util.DataConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GenerateToolApp.Common.ORMEnum;

namespace GenerateToolApp.Common
{
    /// <summary>
    /// ORM帮助类
    /// </summary>
    public class ORMUtil
    {
        /// <summary>
        /// 根据数据库类型获取系统类型
        /// </summary>
        /// <param name="dbTypeString">数据库类型</param>
        /// <returns></returns>
        public static string GetSystemTypeString(string dbTypeString)
        {
            switch (dbTypeString.ToLower())
            {
                case "int":
                    return "int";

                case "varchar":
                    return "string";

                case "bit":
                    return "bool";

                case "datetime":
                    return "DateTime";

                case "decimal":
                    return "decimal";

                case "float":
                    return "float";

                case "image":
                    return "object";

                case "money":
                    return "decimal";

                case "ntext":
                    return "string";

                case "nvarchar":
                    return "string";

                case "smalldatetime":
                    return "DateTime";

                case "smallint":
                    return "int";

                case "text":
                    return "string";

                case "bigint":
                    return "long";

                case "binary":
                    return "object";

                case "char":
                    return "string";

                case "nchar":
                    return "string";

                case "numeric":
                    return "decimal";

                case "real":
                    return "Single";

                case "smallmoney":
                    return "decimal";

                case "timestamp":
                    return "DateTime";

                case "tinyint":
                    return "int";

                case "uniqueidentifier":
                    return "Guid";

                case "varbinary":
                    return "object";

                case "time":
                    return "DateTime";

                case "double":
                    return "double";
            }
            return "string";
        }
        /// <summary>
        /// 获取数据库类型默认值
        /// </summary>
        /// <param name="dbTypeString">SQL类型</param>
        /// <param name="dbDefaultValue">默认值</param>
        /// <returns></returns>
        public static string GetDefaultValueString(string dbTypeString, string dbDefaultValue)
        {
            if (string.IsNullOrEmpty(dbDefaultValue)) return dbDefaultValue;
            switch (dbTypeString.ToLower())
            {
                case "int":
                    return " = " + dbDefaultValue;

                case "varchar":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "bit":
                    return " = " + (dbDefaultValue.ToLower() == "true" || dbDefaultValue == "1" || dbDefaultValue == "b'1'").ToString().ToLower();

                case "datetime":
                    return " = " + (dbDefaultValue.ToUpper() == "CURRENT_TIMESTAMP" ? "DateTime.Now" : $"\"{dbDefaultValue}\".PackDateTime()");

                case "decimal":
                    return " = " + dbDefaultValue + "m";

                case "float":
                    return " = " + dbDefaultValue + "f";

                case "image":
                    return " = new object()";

                case "money":
                    return " = " + dbDefaultValue + "m";

                case "ntext":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "nvarchar":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "smalldatetime":
                    return " = " + (dbDefaultValue.ToUpper() == "CURRENT_TIMESTAMP" ? "DateTime.Now" : $"\"{dbDefaultValue}\".PackDateTime()");

                case "smallint":
                    return " = " + dbDefaultValue;

                case "text":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "bigint":
                    return " = " + dbDefaultValue;

                case "binary":
                    return " = new object()";

                case "char":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "nchar":
                    return " = \"" + dbDefaultValue.Trim('\'').Trim('\"') + "\"";

                case "numeric":
                    return " = " + dbDefaultValue + "m";

                case "double":
                    return " = " + dbDefaultValue;

                case "real":
                    return " = " + dbDefaultValue;

                case "smallmoney":
                    return " = " + dbDefaultValue + "m";

                case "timestamp":
                    return " = " + (dbDefaultValue.ToUpper() == "CURRENT_TIMESTAMP" ? "DateTime.Now" : $"\"{dbDefaultValue}\".PackDateTime()");

                case "tinyint":
                    return " = " + dbDefaultValue;

                case "uniqueidentifier":
                    return " = Guid.NewGuid";

                case "varbinary":
                    return " = new object()";

                case "time":
                    return " = " + (dbDefaultValue.ToUpper() == "CURRENT_TIMESTAMP" ? "DateTime.Now" : $"\"{dbDefaultValue}\".PackDateTime()");
            }
            return dbDefaultValue;
        }

        /// <summary>
        /// 获取转换方法
        /// </summary>
        /// <param name="sqlTypeString">数据库类型</param>
        /// <returns></returns>
        public static string GetPackTypeString(string sqlTypeString)
        {
            switch (sqlTypeString.ToLower())
            {
                case "int":
                    return "PackInt";

                case "varchar":
                    return "PackString";

                case "bit":
                    return "PackInt";

                case "datetime":
                    return "PackDateTime";

                case "decimal":
                    return "PackDecimal";

                case "float":
                    return "PackFloat";

                case "money":
                    return "PackDecimal";

                case "ntext":
                    return "PackString";

                case "nvarchar":
                    return "PackString";

                case "smalldatetime":
                    return "PackDateTime";

                case "smallint":
                    return "PackInt";

                case "text":
                    return "PackString";

                case "bigint":
                    return "PackLong";

                case "char":
                    return "PackString";

                case "nchar":
                    return "PackString";

                case "numeric":
                    return "PackDecimal";

                case "real":
                    return "PackSingle";

                case "smallmoney":
                    return "PackDecimal";

                case "timestamp":
                    return "PackDateTime";

                case "tinyint":
                    return "PackInt";

                case "uniqueidentifier":
                    return "PackGuid";

                case "time":
                    return "PackDateTime";
            }
            return "PackString";
        }
        /// <summary>
        /// 获取转换方法
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string GetPackTypeStringByCodeType(string codeType)
        {
            switch (codeType.ToLower())
            {
                case "int":
                    return "PackInt";

                case "long":
                    return "PackLong";
                case "string":
                    return "PackString";

                case "datetime":
                    return "PackDateTime";

                case "decimal":
                    return "PackDecimal";
            }
            return "PackString";
        }

        /// <summary>
        /// 获取转换方法
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string GetConventTypeString(string codeType)
        {
            switch (codeType.ToLower())
            {
                case "int":
                    return "ConvertToInt";

                case "long":
                    return "ConvertToLong";
                case "string":
                    return "ConvertToString";

                case "datetime":
                    return "ConvertToDateTime";

                case "decimal":
                    return "ConvertDecimal";
            }
            return "ConvertToString";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string GetEmptyCheckString(string columnName, string codeType)
        {
            var a = DateTime.Now;
            switch (codeType.ToLower())
            {
                case "int":
                    return $@"if({columnName}>0)";

                case "long":
                    return $@"if({columnName}>0)";
                case "string":
                    return $@"if(!string.IsNullOrEmpty({columnName}))";

                case "datetime":
                    return $@" if ({columnName} > ""1900-01-01"".PackDateTime())";

                case "decimal":
                    return $@"if({columnName}>0)";
            }
            return "ConvertToString";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlComment"></param>
        /// <returns></returns>
        public static Tuple<UIControlsType, string, bool> AnlysisComment(string sqlComment)
        {
            if (string.IsNullOrEmpty(sqlComment))
            {
                return new Tuple<UIControlsType, string, bool>(UIControlsType.input, "", false);
            }

            var arr = sqlComment.Split('|');
            if (arr.Length <= 1 || string.IsNullOrEmpty(arr[1]))
            {
                return new Tuple<UIControlsType, string, bool>(UIControlsType.input, arr[0], false);
            }

            var isRowFlag = arr.Length >= 3 && arr[2].PackString() == "标记";

            switch (arr[1].ToLower())
            {
                case "input":
                    return new Tuple<UIControlsType, string, bool>(UIControlsType.input, arr[0], isRowFlag);
                case "select":
                    return new Tuple<UIControlsType, string, bool>(UIControlsType.select, arr[0], isRowFlag);
                case "checkbox":
                    return new Tuple<UIControlsType, string, bool>(UIControlsType.checkbox, arr[0], isRowFlag);
                case "radio":
                    return new Tuple<UIControlsType, string, bool>(UIControlsType.radio, arr[0], isRowFlag);
                case "date":
                    return new Tuple<UIControlsType, string, bool>(UIControlsType.date, arr[0], isRowFlag);
            }

            return new Tuple<UIControlsType, string, bool>(UIControlsType.input, arr[0], isRowFlag);
        }

        /// <summary>
        /// 获取标准表每一列的列表
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<ORMColumn> GetOrmColumnList(dynamic tables)
        {
            var columnList = new List<ORMColumn>();
            if (tables == null)
            {
                return columnList;
            }
            var cIndex = 0;
            foreach (var item in tables)
            {
                var re = AnlysisComment(item.COLUMN_COMMENT);
                columnList.Add(new ORMColumn
                {
                    CIndex = ++cIndex,
                    CodeType = GetSystemTypeString(item.DATA_TYPE),
                    Defalut = GetDefaultValueString(item.DATA_TYPE, item.COLUMN_DEFAULT),
                    DbType = item.COLUMN_TYPE,
                    IsNull = item.IS_NULLABLE != "NO",
                    Comment = re.Item2,
                    IsPrimaryKey = item.COLUMN_KEY == "PRI",
                    Name = item.COLUMN_NAME,
                    UIControls = re.Item1,
                    IsRowFlag = re.Item3


                });
            }

            return columnList;
        }

    }

    /// <summary>
    /// where条件
    /// </summary>
    public class WherePredicate
    {
        /// <summary>
        /// 查询列名
        /// </summary>
        public Enum field { get; set; }

        /// <summary>
        /// 操作条件
        /// </summary>
        public ConditionEnum conditionEnum { get; set; }

    }
}
