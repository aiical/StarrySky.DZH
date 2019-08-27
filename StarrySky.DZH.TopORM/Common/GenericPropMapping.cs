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
    public class GenericPropMapping
    {
        public static Dictionary<string, List<PropConstruction>> GenericClassDic = new Dictionary<string, List<PropConstruction>>();

        public static readonly object Locker = new object();

        public static List<PropConstruction> GetProp<T>(T classModel)
        {
            List<PropConstruction> result = new List<PropConstruction>();
            Type type = typeof(T);
            var props = type.GetProperties();
            string fullName = type.FullName;
            if (GenericClassDic == null || !GenericClassDic.Any() || !GenericClassDic.Keys.Contains(fullName))
            {
                lock (Locker)
                {
                    if (GenericClassDic == null || !GenericClassDic.Any() || !GenericClassDic.Keys.Contains(fullName))
                    {
                        if (props != null && props.Any())
                        {
                            foreach (var p in props)
                            {
                                result.Add(new PropConstruction()
                                {
                                    PropInfo=p,
                                    PropName = p.Name,
                                    TypeName =p.PropertyType.Name,
                                    TypeFullName=p.PropertyType.FullName,
                                    AttrList=p.CustomAttributes.Select(x=>x.AttributeType.Name).ToList()
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
        /// 特性描述列表
        /// </summary>
        public List<string> AttrList { get; set; } = new List<string>();
    }
}
