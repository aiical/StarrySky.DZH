using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.ORMFramework
{
    /// <summary>
    /// 字段实体属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class TableInfoAttribute : Attribute
    {
        readonly string _dataBaseName;
        readonly string _tableName;
        readonly string _dataBaseType;

        /// <summary>
        /// 实体属性
        /// </summary>
        /// <param name="dataBaseName">数据库名</param>
        /// <param name="tableName">表名</param>
        /// <param name="dataBaseType">数据库类型</param>
        public TableInfoAttribute(string dataBaseName, string tableName, string dataBaseType)
        {
            this._dataBaseName = dataBaseName;
            this._tableName = tableName;
            this._dataBaseType = dataBaseType;
        }
        /// <summary>
        /// 实体属性
        /// </summary>
        /// <param name="dataBaseName">数据库名</param>
        /// <param name="tableName">表名</param>
        public TableInfoAttribute(string dataBaseName, string tableName)
        {
            this._dataBaseName = dataBaseName;
            this._tableName = tableName;
            this._dataBaseType = "SQLServer";
        }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBaseName
        {
            get { return _dataBaseName; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DataBaseType
        {
            get { return _dataBaseType; }
        }
    }

    /// <summary>
    /// 主键属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// 分片键属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ShardKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// 实体属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DataBaseAttribute : Attribute
    {
        readonly string _dataBaseName;

        /// <summary>
        /// 实体属性
        /// </summary>
        /// <param name="dataBaseName">数据库名</param>
        public DataBaseAttribute(string dataBaseName)
        {
            this._dataBaseName = dataBaseName;
        }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBaseName
        {
            get { return _dataBaseName; }
        }
    }
    /// <summary>
    /// 数据库类型类型配置属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DataBaseConfigAttribute : Attribute
    {
        readonly string _dataBaseType;

        /// <summary>
        /// 实体属性
        /// </summary>
        /// <param name="dataBaseType">数据库类型</param>
        public DataBaseConfigAttribute(string dataBaseType)
        {
            this._dataBaseType = dataBaseType;
        }
        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public string DataBaseType
        {
            get { return _dataBaseType; }
        }
    }
    /// <summary>
    /// 属性帮助类
    /// </summary>
    public class DALAttrHelper
    {
        /// <summary>
        /// 根据类型获取表名
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetTableName(Type t)
        {
            var obj = t.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (obj == null) return string.Empty;
            var ma = (TableAttribute)obj;
            return ma.Name;
        }
        /// <summary>
        /// 根据实体获取数据库名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetTableName<T>(T entity)
        {
            if (entity == null) return string.Empty;
            return GetTableName(entity.GetType());
        }
        /// <summary>
        /// 根据类型获取数据库名称
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDataBase(Type t)
        {
            var obj = t.GetCustomAttributes(typeof(DataBaseAttribute), false).FirstOrDefault();
            if (obj == null) return string.Empty;
            var ma = (DataBaseAttribute)obj;
            return ma.DataBaseName;
        }
        /// <summary>
        /// 根据实体获取数据库名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetDataBase<T>(T entity)
        {
            if (entity == null) return string.Empty;
            return GetDataBase(entity.GetType());
        }

        /// <summary>
        /// 根据字段获取表信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static dynamic GetTableNameByFields(Enum field)
        {
            var obj = field.GetType().GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault();
            if (obj == null) return new { DataBaseName = "", TableName = "", TableFullName = "" };
            var ma = (TableInfoAttribute)obj;
            return new { DataBaseName = ma.DataBaseName, TableName = ma.TableName, TableFullName = string.Format("{0}.dbo.{1}", ma.DataBaseName, ma.TableName) };
        }

        /// <summary>
        /// 根据实体类型获取主键字段
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns></returns>
        public static string GetKey(Type t)
        {
            PropertyInfo[] fields = t.GetProperties();
            //检索所有字段
            foreach (PropertyInfo field in fields)
            {
                KeyAttribute da = null;
                object[] arrobj = field.GetCustomAttributes(typeof(KeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as KeyAttribute;
                if (da != null) return field.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据实体类型获取分片键字段
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetShardKeys(Type t)
        {
            List<string> keyList = new List<string>();
            PropertyInfo[] fields = t.GetProperties();
            //检索所有字段
            foreach (PropertyInfo field in fields)
            {
                ShardKeyAttribute da = null;
                object[] arrobj = field.GetCustomAttributes(typeof(ShardKeyAttribute), true);
                if (arrobj.Length > 0) da = arrobj[0] as ShardKeyAttribute;
                if (da != null) keyList.Add(field.Name);
            }
            return keyList;
        }

        /// <summary>
        /// 根据实体获取数据库类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GetDataBaseType<T>(T entity)
        {
            return GetDataBaseType(entity.GetType());
        }
        /// <summary>
        /// 根据类型获取数据库名称
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDataBaseType(Type t)
        {
            var obj = t.GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault();
            if (obj == null) return string.Empty;
            var ma = (TableInfoAttribute)obj;
            return ma.DataBaseType;
        }
        /// <summary>
        /// 根据字段获取数据库配置信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetDataBaseTypeByFields(Enum field)
        {
            var obj = field.GetType().GetCustomAttributes(typeof(TableInfoAttribute), false).FirstOrDefault();
            if (obj == null) return string.Empty;
            var ma = (TableInfoAttribute)obj;
            return ma.DataBaseType;
        }
    }
}