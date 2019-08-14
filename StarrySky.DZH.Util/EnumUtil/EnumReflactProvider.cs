using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.EnumUtil
{
    /// <summary>
    /// 枚举反射到静态字典中获取描述
    /// </summary>
    public static class EnumReflactProvider
    {
        public static Dictionary<string, Dictionary<int, string>> enumDic = new Dictionary<string, Dictionary<int, string>>();

        private static readonly object Locker = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumDescriptions(Enum en)
        {
            string desc = "";
            try
            {
                string enumFullName = en.GetType().FullName;
                if (enumDic == null || !enumDic.Any() || !enumDic.Keys.Contains(enumFullName))
                {
                    lock (Locker)
                    {
                        if (enumDic == null || !enumDic.Any() || !enumDic.Keys.Contains(enumFullName))
                        {
                            Dictionary<int, String> enumFieldDic = new Dictionary<int, string>();
                            foreach (Enum enumValue in Enum.GetValues(en.GetType()))
                            {
                                int key = enumValue.GetHashCode();
                                string description = enumValue.GetEnumDescription();
                                enumFieldDic.Add(key, description);
                            }
                            enumDic.Add(enumFullName, enumFieldDic);
                        }
                    }
                }
                Dictionary<int, String> result = new Dictionary<int, string>();
                enumDic.TryGetValue(enumFullName, out result);
                result.TryGetValue(en.GetHashCode(), out desc);
            }
            catch (Exception ex)
            {
                
            }
            return desc ?? "";
        }
    }
}
