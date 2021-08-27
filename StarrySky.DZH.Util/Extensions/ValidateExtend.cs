using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Extensions
{
    /// <summary>
    /// 验证扩展类
    /// </summary>
    public static class ValidateExtend
    {
        #region sql字符串验证

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(this string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        #endregion

        #region 数字验证
        /// <summary>
        /// 最多一位小数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsDecimalOne(this string str)
        {
            //最多一位小数
            if (string.IsNullOrWhiteSpace(str))
                return false;
            return Regex.IsMatch(str, @"^\d+(\.\d){0,1}$");

        }
        /// <summary>
        /// 两位小数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsDecimalTwo(this string str)
        {
            //最多两位小数
            if (string.IsNullOrWhiteSpace(str))
                return false;
            return Regex.IsMatch(str, @"^\\d+(\\.\\d{0,2})?$");
        }
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="strNum">待测试的字符串</param>
        /// <returns>是则返回true,否则返回false</returns>
        public static bool IsNumeric(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            return Regex.IsMatch(str, "^[0-9]+$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            return Regex.IsMatch(str, @"^[+-]?\d*$");
        }
        #endregion

        #region 汉字验证
        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="strWords">待测试的字符串</param>
        /// <returns>是则返回true;否则返回false</returns>
        public static bool IsChineseWord(this string strWords)
        {
            if (strWords == null)
                return false;
            return strWords.All(charWord => Regex.IsMatch(charWord.ToString(), "[\u4e00-\u9fa5]+"));
        }

        /// <summary>
        /// 判断是否中文字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsChinese(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return new Regex("^[一-龥]+$", RegexOptions.None).IsMatch(s);
        }

        /// <summary>
        /// 判断字符串内是否有中文
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsBearWithChinese(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return new Regex("[一-龥]+", RegexOptions.None).IsMatch(s);
        }
        #endregion


        /// <summary>
        /// 验证字符串是否网站
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsWebsite(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return new Regex(@"^((http|https|ftp)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.IgnoreCase).IsMatch(s);
        }

        /// <summary>
        /// 验证字符串是否邮编
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsZipCode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return new Regex(@"^[1-9]\d{5}(?!\d)$", RegexOptions.None).IsMatch(s);
        }

        #region EMail验证
        /// <summary>
        /// 是否是电子邮件
        /// 如果满足电邮格式则返回tue,否则返回false
        /// </summary>
        /// <param name="strMail">邮件地址</param>
        /// <returns>如果满足电邮格式则返回tue,否则返回false</returns>
        public static bool IsEMail(this string strMail)
        {
            if (strMail == null)
                return false;
            return Regex.IsMatch(strMail.Trim(), @"^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$");
        }
        #endregion

        #region 手机验证
        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="strMobile">待测试手机号字符串</param>
        /// <returns>是手机格式就返回true;否则返回false</returns>
        public static bool IsMobile(this string strMobile)
        {
            if (strMobile == null)
                return false;
            return Regex.IsMatch(strMobile.Trim(), @"^(1)\d{10}$");

        }
        #endregion

        #region 电话号码验证
        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns>是电话格式就返回true;否则返回false</returns>
        public static bool IsPhone(this string strPhone)
        {
            if (strPhone == null)
                return false;
            return Regex.IsMatch(strPhone.Trim(), @"^(\+86\s{1,1})?((\d{3,4}\-)\d{7,8})$");
        }
        #endregion

        #region 身份证验证
        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(this string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(this string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(this string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        #endregion

        #region 常用空验证
        /// <summary>
        /// 验证字符是否为空
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>true:空 false:非空</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        /// <summary>
        /// 验证StringBuilder是否为空
        /// </summary>
        /// <param name="str">StringBuilder</param>
        /// <returns>true:空 false:非空</returns>
        public static bool IsNullOrEmpty(this StringBuilder str)
        {
            return str == null ? false : str.Length < 1;
        }
        /// <summary>
        /// 验证DataRow是否为空
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="ColumnName">列名</param>
        /// <returns>true:空 false:非空</returns>
        public static bool IsNullOrEmpty(this DataRow dr, string ColumnName)
        {
            return (dr == null || dr.IsNull(ColumnName) || dr[ColumnName].ToString().IsNullOrEmpty());
        }

        /// <summary>
        /// 判断一个集合是否为空或者未初始化
        /// </summary>
        /// <typeparam name="T">任意Model</typeparam>
        /// <param name="list">任意实现Ilist接口的类型</param>
        /// <returns>返回true表示空，否则不为空</returns>
        public static bool CollectionIsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
        
        #endregion
    }
}
