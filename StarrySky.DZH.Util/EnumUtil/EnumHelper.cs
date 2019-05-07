using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.EnumUtil
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {
        #region private
        /// <summary>
        /// 获取字段Description
        /// </summary>
        /// <param name="fieldInfo">FieldInfo</param>
        /// <returns>DescriptionAttribute[] </returns>
        private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 从枚举中获取Description
        /// </summary>
        /// <param name="e">需要获取枚举描述的枚举</param>
        /// <returns>描述内容Description</returns>
        public static string GetEnumDescription(this Enum e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            Type enumType = e.GetType();
            DescriptionAttribute attr = null;

            // 获取枚举字段。
            FieldInfo fieldInfo = enumType.GetField(e.ToString());
            if (fieldInfo != null)
            {
                // 获取描述的属性。
                attr = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;
            }

            // 返回结果
            if (attr != null && !string.IsNullOrEmpty(attr.Description))
                return attr.Description;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strEnum"></param>
        /// <returns></returns>
        public static bool TryParseToEnum<T>(string strEnum, out T e)
        {
            try
            {
                e = (T)Enum.Parse(typeof(T), strEnum);
                return true;
            }
            catch (Exception ex)
            {
                e = default(T);
                return false;
            }
        }

        /// <summary>
        /// 根据Description获取枚举
        /// 说明：转失败会抛异常
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">枚举描述</param>
        /// <returns>枚举</returns>
        public static T GetEnum<T>(string description)
        {
            Type _type = typeof(T);
            foreach (FieldInfo field in _type.GetFields())
            {
                DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
                if (_curDesc != null && _curDesc.Length > 0)
                {
                    if (_curDesc[0].Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
        }

        /// <summary>
        /// 将枚举转换为ArrayList
        /// 说明：若不是枚举类型，则返回NULL
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>ArrayList</returns>
        public static ArrayList ToArrayList(this Type type)
        {
            if (type.IsEnum)
            {
                ArrayList _array = new ArrayList();
                Array _enumValues = Enum.GetValues(type);
                foreach (Enum value in _enumValues)
                {
                    _array.Add(new KeyValuePair<Enum, string>(value, GetEnumDescription(value)));
                }
                return _array;
            }
            return null;
        }




        /// <summary>
        /// 获取枚举描述列表，并转化为键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isHasAll">是否包含“全部”</param>
        /// <param name="filterItem">过滤项</param>
        /// <returns></returns>
        public static List<EnumKeyValue> EnumDescToList<T>(bool isHasAll, params string[] filterItem)
        {
            List<EnumKeyValue> list = new List<EnumKeyValue>();

            // 如果包含全部则添加
            if (isHasAll)
            {
                list.Add(new EnumKeyValue() { Key = 0, Name = "全部" });
            }

            #region 方式一
            foreach (var item in typeof(T).GetFields())
            {
                // 获取描述
                var attr = item.GetCustomAttribute(typeof(DescriptionAttribute), true) as DescriptionAttribute;
                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    // 跳过过滤项
                    if (Array.IndexOf<string>(filterItem, attr.Description) != -1)
                    {
                        continue;
                    }
                    // 添加
                    EnumKeyValue model = new EnumKeyValue();
                    model.Key = (int)Enum.Parse(typeof(T), item.Name);
                    model.Name = attr.Description;
                    list.Add(model);
                }
            }
            #endregion

            #region 方式二
            //foreach (int item in Enum.GetValues(typeof(T)))
            //{
            //    // 获取描述
            //    FieldInfo fi = typeof(T).GetField(Enum.GetName(typeof(T), item));
            //    var attr = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
            //    if (attr != null && !string.IsNullOrEmpty(attr.Description))
            //    {
            //        // 跳过过滤项
            //        if (Array.IndexOf<string>(filterItem, attr.Description) != -1)
            //        {
            //            continue;
            //        }
            //        // 添加
            //        EnumKeyValue model = new EnumKeyValue();
            //        model.Key = item;
            //        model.Name = attr.Description;
            //        list.Add(model);
            //    }
            //} 
            #endregion

            return list;
        }

        /// <summary>
        /// 获取枚举值列表，并转化为键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isHasAll">是否包含“全部”</param>
        /// <param name="filterItem">过滤项</param>
        /// <returns></returns>
        public static List<EnumKeyValue> EnumToList<T>(bool isHasAll, params string[] filterItem)
        {
            List<EnumKeyValue> list = new List<EnumKeyValue>();

            // 如果包含全部则添加
            if (isHasAll)
            {
                list.Add(new EnumKeyValue() { Key = 0, Name = "全部" });
            }

            foreach (int item in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), item);
                // 跳过过滤项
                if (Array.IndexOf<string>(filterItem, name) != -1)
                {
                    continue;
                }
                // 添加
                EnumKeyValue model = new EnumKeyValue();
                model.Key = item;
                model.Name = name;
                list.Add(model);
            }

            return list;
        }
    }

    /// <summary>
    /// 枚举键值对
    /// </summary>
    public class EnumKeyValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
