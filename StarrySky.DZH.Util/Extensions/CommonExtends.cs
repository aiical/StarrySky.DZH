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

        /// <summary>
        /// 扩展字符串包含某些特定字符串，只要包含一个即为true,否则false
        /// </summary>
        /// <param name="str">要匹配的字符</param>
        /// <param name="matchColle">匹配集合</param>
        /// <returns>true 包含， fasle 不包含</returns>
        public static bool ContainsOneOfList(this string str, List<string> matchColle)
        {
            if (matchColle.CollectionIsNullOrEmpty())
            {
                return false;
            }

            return matchColle.Exists(x => str.Contains(x));
        }

        /// <summary>
        /// 扩展字符串包含某些特定字符串，只要包含一个即为true,否则false
        /// </summary>
        /// <param name="str">要匹配的字符</param>
        /// <param name="matchSpecials">匹配集合,只接受英文逗号隔开的字符串</param>
        /// <returns>true 包含， fasle 不包含</returns>
        public static bool ContainsOneOfSpecial(this string str, string matchSpecials)
        {
            if (matchSpecials.IsNullOrWhiteSpace())
            {
                return false;
            }

            var matchColle = matchSpecials.Split(',').ToList();
            return matchColle.Exists(x => str.Contains(x));
        }

        /// <summary>
        /// 扩展字符串等于某些特定字符串，只要等于一个即为true,否则false
        /// </summary>
        /// <param name="str">要匹配的字符</param>
        /// <param name="matchSpecials">匹配集合,只接受英文逗号隔开的字符串</param>
        /// <returns>true 等于， fasle 不等于</returns>
        public static bool EqualsOneOfSpecial(this string str, string matchSpecials)
        {
            if (matchSpecials.IsNullOrWhiteSpace())
            {
                return false;
            }

            var matchColle = matchSpecials.Split(',').ToList();
            return matchColle.Exists(x => str.Equals(x));
        }
    }
}
