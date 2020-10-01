using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Extensions
{
    /// <summary>
    /// 通用扩展 无处安放的
    /// </summary>
    public static class CommonExtends
    {
        /// <summary>
        /// 扩展join拼接字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">list</param>
        /// <param name="separator">分隔符号</param>
        /// <returns>returns</returns>
        public static string JoinByseparator<T>(this List<T> list, string separator)
        {
            return list.CollectionIsNullOrEmpty() ? string.Empty : string.Join(separator, list);
        }

        /// <summary>
        /// 扩展list 条件add
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">list</param>
        /// <param name="condition">条件</param>
        /// <param name="item">添加元素</param>
        public static void AddIF<T>(this List<T> list, bool condition, T item)
        {
            if (condition)
            {
                list.Add(item);
            }
        }
    }
}
