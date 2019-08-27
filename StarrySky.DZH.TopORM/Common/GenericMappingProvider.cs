using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.TopORM.Common
{
    /// <summary>
    /// 映射泛型属性到内存
    /// </summary>
    public class GenericMappingProvider
    {
        public static Dictionary<string, ClassConstruction> GenericClassDic = new Dictionary<string, ClassConstruction>();

        public static readonly object Locker = new object();
        /// <summary>
        /// 获取泛型属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classModel"></param>
        /// <returns></returns>
        public static ClassConstruction GetClassConstruction<T>(T classModel)
        {

            ClassConstruction result = new ClassConstruction();
            Type type = typeof(T);
            var props = type.GetProperties();
            string fullName = type.FullName;
            if (GenericClassDic == null || !GenericClassDic.Any() || !GenericClassDic.Keys.Contains(fullName))
            {
                lock (Locker)
                {
                    if (GenericClassDic == null || !GenericClassDic.Any() || !GenericClassDic.Keys.Contains(fullName))
                    {
                        result.AttributeList = type.GetCustomAttributes().ToList();
                        if (props != null && props.Any())
                        {
                            foreach (var p in props)
                            {
                                result.Properities.Add(new PropConstruction()
                                {
                                    PropInfo = p,
                                    PropName = p.Name,
                                    TypeName = p.PropertyType.Name,
                                    TypeFullName = p.PropertyType.FullName,
                                    AttrNameList = p.CustomAttributes.Select(x => x.AttributeType.Name).ToList()
                                });
                            }
                        }
                        GenericClassDic.Add(fullName, result);
                    }
                }
            }
            else
            {
                GenericClassDic.TryGetValue(fullName, out result);
            }
            return result;
        }
    }
    /// <summary>
    /// 泛型类结构模型
    /// </summary>
    public class ClassConstruction
    {
        /// <summary>
        /// 类的特性描述列表
        /// </summary>
        public List<Attribute> AttributeList { get; set; } = new List<Attribute>();
        /// <summary>
        /// 所有属性集合
        /// </summary>
        public List<PropConstruction> Properities { get; set; } = new List<PropConstruction>();

    }
    /// <summary>
    /// 属性结构模型
    /// </summary>
    public class PropConstruction
    {
        /// <summary>
        /// 原生
        /// </summary>
        public PropertyInfo PropInfo { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropName { get; set; }
        /// <summary>
        /// 属性类型名（int64等）
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 属性类型全名
        /// </summary>
        public string TypeFullName { get; set; }
        /// <summary>
        /// 属性的特性描述列表
        /// </summary>
        public List<string> AttrNameList { get; set; } = new List<string>();
    }
}
