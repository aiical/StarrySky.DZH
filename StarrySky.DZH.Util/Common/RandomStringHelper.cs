using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class RandomStringHelper
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，</param>
        /// <param name="useLow">是否包含小写字母，</param>
        /// <param name="useUpp">是否包含大写字母，</param>
        /// <param name="useSpe">是否包含特殊字符，默认为不包含</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            StringBuilder s = new StringBuilder(), strRanges = new StringBuilder();
            if (!useNum && !useLow && !useUpp && !useSpe)
            {
                return null;
            }

            if (useNum)
            {
                strRanges.Append("0123456789");
            }

            if (useLow)
            {
                strRanges.Append("abcdefghijklmnopqrstuvwxyz");
            }

            if (useUpp)
            {
                strRanges.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            }

            if (useSpe)
            {
                strRanges.Append("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            }

            var rules = strRanges.ToString();
            for (int i = 0; i < length; i++)
            {
                s.Append(rules.Substring(r.Next(0, rules.Length - 1), 1));
            }

            return s.ToString();
        }
    }
}
