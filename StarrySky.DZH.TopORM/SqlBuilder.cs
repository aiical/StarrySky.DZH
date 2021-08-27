using StarrySky.DZH.TopORM.Common;
using StarrySky.DZH.TopORM.CustomAttribute;
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

        public static string ToInsertSql<T>(T obj, out PropConstruction prop)
        {
            prop = null;
            StringBuilder sbInsertSql = new StringBuilder();
            Type type = typeof(T);
            var classConstruction = GenericMappingProvider.GetClassConstruction(obj);
            if (classConstruction.Properities.CollectionIsNullOrEmpty()) {
                return "";
            }
            List<string> columnName = new List<string>();
            foreach (var p in classConstruction.Properities)
            {
                if (p.AttrNameList.Contains(typeof(PrimaryKeyAttribute).Name))
                {
                    prop = p;
                }
                else if(!p.AttrNameList.Contains(typeof(IgnoreFieldAttribute).Name))
                {
                    columnName.Add(p.PropName);
                }
            }
            var tableInfo = (TableInfoAttribute)classConstruction.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
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
            PropConstruction keyProp = null;
            StringBuilder sbSql = new StringBuilder();
            Type type = typeof(T);
            var classConstruction = GenericMappingProvider.GetClassConstruction(obj);
            if (classConstruction.Properities.CollectionIsNullOrEmpty())
            {
                return "";
            }
            List<string> setcolumn = new List<string>();
            foreach (var p in classConstruction.Properities)
            {
                
                if (p.AttrNameList.Contains(typeof(PrimaryKeyAttribute).Name))
                {
                    keyProp = p;
                    continue;
                }
                if (p.AttrNameList.Contains(typeof(IgnoreFieldAttribute).Name))
                {
                    continue;
                }
                if (GetChangeStage(obj, p.PropInfo))
                {
                    setcolumn.Add($"{p.PropName}=@{p.PropName}");
                }
            }
            var tableInfo = (TableInfoAttribute)classConstruction.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
            if (keyProp == null||(long)keyProp.PropInfo.GetValue(obj)==0L || setcolumn.CollectionIsNullOrEmpty())
            {
                return "";
            }
            sbSql.AppendFormat($"UPDATE {tableInfo.TableName} SET {string.Join(",", setcolumn)} where {keyProp.PropName}=@{keyProp.PropName}");
            return sbSql.ToString();
        }

        public static string ToDeleteSql<T>(T obj) where T:class
        {
            var type = typeof(T);
            var classConstruction = GenericMappingProvider.GetClassConstruction(obj);
            if (classConstruction.Properities.CollectionIsNullOrEmpty())
            {
                return "";
            }
            var tableInfo = (TableInfoAttribute)classConstruction.AttributeList.FirstOrDefault(x => x.TypeId.ToString().Contains(nameof(TableInfoAttribute)));
            string primaryKey = "";
            foreach (var p in classConstruction.Properities)
            {
                if (p.AttrNameList.Contains(nameof(PrimaryKeyAttribute)))
                {
                    primaryKey = p.PropName;
                    break;
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
