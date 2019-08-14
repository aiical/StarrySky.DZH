﻿using StarrySky.DZH.TopORM.CustomAttribute;
using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM
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
        /// 全字段更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string ToUpdateSql<T>(T obj)
        {
            PropertyInfo keyProp = null;
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
                    keyProp = p;
                    continue;
                }
                if (!Ignore.IsNullOrEmptyCollection())
                {
                    continue;
                }
                if (GetChangeStage(obj, p))
                {
                    setcolumn.Add($"{p.Name}=@{p.Name}");
                }
            }
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());
            if (keyProp == null||(long)keyProp.GetValue(obj)==0L || setcolumn.IsNullOrEmptyCollection())
            {
                return "";
            }
            sbSql.AppendFormat($"UPDATE {tableInfo.TableName} SET {string.Join(",", setcolumn)} where {keyProp.Name}=@{keyProp.Name}");
            return sbSql.ToString();
        }

        public static string ToDeleteSql<T>(T obj, out PropertyInfo prop)
        {
            prop = null;
            var type = typeof(T);
            var tableInfo = (TableInfoAttribute)(type.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault());
            var properties = type.GetProperties();//获取属性
            if (properties.IsNullOrEmptyCollection())
            {
                return "";
            }
            string primaryKey = "";
            foreach (var p in properties)
            {
                object[] PrimaryKey = p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);

                if (!PrimaryKey.IsNullOrEmptyCollection())
                {
                    prop = p;
                    primaryKey = p.Name;
                }
            }
            return $"delete from {tableInfo.TableName} where {primaryKey}=@Key";
        }
        /// <summary>
        /// 列是否修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool GetChangeStage<T>(T obj, PropertyInfo p)
        {
            var value = p.GetValue(obj);
            if (p.PropertyType == typeof(string) && (string)value == default(string))
            {
                return false;
            }
            if (p.PropertyType == typeof(DateTime) && (DateTime)value == default(DateTime))
            {
                return false;
            }
            if (p.PropertyType == typeof(int) && (int)value == default(int))
            {
                //return false;
            }
            return true;
        }


    }
}