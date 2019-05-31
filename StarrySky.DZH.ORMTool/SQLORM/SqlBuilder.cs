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
        public static void TestReflect<T>(T obj)
        {
            //GetType()方法继承自Object，所以C#中任何对象都具有GetType()方法，x.GetType()，其中x为变量名
            //typeof(x)中的x，必须是具体的类名、类型名称等，不可以是变量名称           
            var type1 = obj.GetType();//{Name = "DemoEntity" FullName = "StarrySky.DZH.ORMTool.SQLORM.Entity.DemoEntity"}
            var type2 = obj.GetType().GetTypeInfo();//{Name = "DemoEntity" FullName = "StarrySky.DZH.ORMTool.SQLORM.Entity.DemoEntity"}
            Type type = typeof(T);//{Name = "DemoEntity" FullName = "StarrySky.DZH.ORMTool.SQLORM.Entity.DemoEntity"}
            var ss = type.Name;
            var type3 = type.GetType();//{Name = "RuntimeType" FullName = "System.RuntimeType"}
            var type4 = type.GetTypeInfo();//{Name = "DemoEntity" FullName = "StarrySky.DZH.ORMTool.SQLORM.Entity.DemoEntity"}
            var properties = type.GetProperties();//获取属性
            var s1 = properties.First().PropertyType;//{Name = "Int64" FullName = "System.Int64"}
            var s2 = properties.First().PropertyType.Name;
            var s3 = properties.First().GetType();//{Name = "RuntimePropertyInfo" FullName = "System.Reflection.RuntimePropertyInfo"}
            var members = type.GetMembers();
            var fields = type.GetFields();//获取公共字段（非属性），包含静态和非静态
        }

        public static string ToInsertSql<T>(T obj, out PropertyInfo prop)
        {
            prop = null;
            StringBuilder sbInsertSql = new StringBuilder();
            Type type = typeof(T);
            var properties = type.GetProperties();//获取属性         
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
            Type type = typeof(T);
            var properties = type.GetProperties();//获取属性
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
                    continue;
                }
                if (!Ignore.IsNullOrEmptyCollection())
                {
                    continue;
                }
                if (GetChangeStagel(obj, p))
                {
                    setcolumn.Add($"{p.Name}=@{p.Name}");
                }
            }
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());

            sbSql.AppendFormat($"UPDATE {tableInfo.TableName} SET {string.Join(",", setcolumn)} where {prop.Name}=@{prop.Name}");

            return sbSql.ToString();
        }


        public static bool GetChangeStagel<T>(T obj, PropertyInfo p)
        {
            var type = p.GetValue(obj);
            if (type is string v && v == default(string))
            {
                return false;
            }


            return true;
        }
    }
}
