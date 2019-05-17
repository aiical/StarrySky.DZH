using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.EnumUtil
{
    /// <summary>
    /// Enum获取所有description 到静态字典中，以后使用遍历字典，减少反射成本
    /// </summary>
    public class EnumDescriptionHelper
    {
        public static Dictionary<int, string> ProjectDic = new Dictionary<int, string>();

        private static readonly object Locker = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public static string GetProjectDescription(int projectCode)
        {
            string desc = "";
            if (ProjectDic == null || !ProjectDic.Any())
            {
                lock (Locker)
                {
                    if (ProjectDic == null || !ProjectDic.Any())
                    {
                        Dictionary<Int32, String> enumDic = new Dictionary<int, string>();
                        foreach (ProjectEnum enumValue in Enum.GetValues(typeof(ProjectEnum)))
                        {
                            int key = enumValue.GetHashCode();
                            string description = enumValue.GetEnumDescription();
                            enumDic.Add(key, description);
                        }
                        ProjectDic = enumDic;
                    }
                }
            }
            ProjectDic.TryGetValue(projectCode, out desc);
            return desc ?? "";
        }
    }
    public enum ProjectEnum
    {
        /// <summary>
        /// 我是A
        /// </summary>
        [Description("我是A")]
        A = 10101,
        /// <summary>
        /// 我是B
        /// </summary>
        [Description("我是B")]
        B = 10102,
        /// <summary>
        /// 我是C
        /// </summary>
        [Description("我是C")]
        C = 10103,
    }
}
   
