using StarrySky.DZH.ORMTool.SQLORM.CustomAttribute;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.SQLORM
{
    public class SqlBuilder
    {
        public static string ToInsertSql<T>(T obj, out PropertyInfo prop)
        {
            prop = null;
            StringBuilder sbInsertSql = new StringBuilder();
            //GetType()方法继承自Object，所以C#中任何对象都具有GetType()方法，x.GetType()，其中x为变量名
            //typeof(x)中的x，必须是具体的类名、类型名称等，不可以是变量名称
            Type type = typeof(T);
            //var type1 = obj.GetType();
            var properties = type.GetProperties();//获取属性
            //var members = type.GetMembers();
            //var fields = type.GetFields();//获取公共字段（非属性），包含静态和非静态
            if (properties.IsNullOrEmptyCollection())
            {
                return "";
            }
            List<string> columnName = new List<string>();
            foreach (var p in properties)
            {
                object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                object[] Ignore = p.GetCustomAttributes(typeof(IgnoreFieldAttribute), false);
                if (!PrimaryKey.IsNullOrEmptyCollection())
                {
                    prop = p;
                }
                if (PrimaryKey.IsNullOrEmptyCollection() && Ignore.IsNullOrEmptyCollection())
                {
                    columnName.Add(p.Name);
                }
            }
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());

            sbInsertSql.AppendFormat($"INSERT INTO {tableInfo.TableName} ({string.Join(",", columnName)}) VALUES  ({string.Join(",", columnName.Select(p => $"@{p}"))})");

            return sbInsertSql.ToString();
        }
        /// <summary>
        /// 无差别全字段更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string ToUpdateSql<T>(T obj, out PropertyInfo prop)
        {
            prop = null;
            StringBuilder sbSql = new StringBuilder();
            //GetType()方法继承自Object，所以C#中任何对象都具有GetType()方法，x.GetType()，其中x为变量名
            //typeof(x)中的x，必须是具体的类名、类型名称等，不可以是变量名称
            Type type = typeof(T);
            //var type1 = obj.GetType();
            var properties = type.GetProperties();//获取属性
            //var members = type.GetMembers();
            //var fields = type.GetFields();//获取公共字段（非属性），包含静态和非静态
            if (properties.IsNullOrEmptyCollection())
            {
                return "";
            }
            List<string> setcolumn = new List<string>();
            foreach (var p in properties)
            {
                object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                object[] Ignore = p.GetCustomAttributes(typeof(IgnoreFieldAttribute), false);
                if (!PrimaryKey.IsNullOrEmptyCollection())
                {
                    prop = p;
                }
                if (PrimaryKey.IsNullOrEmptyCollection() && Ignore.IsNullOrEmptyCollection())
                {
                    setcolumn.Add($"{p.Name}=@{p.Name}");
                }
            }
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());

            sbSql.AppendFormat($"UPDATE {tableInfo.TableName} SET {string.Join(",", setcolumn)} where {prop.Name}=@{prop.Name}");

            return sbSql.ToString();
        }
    }
}
