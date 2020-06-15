using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util
{
    /// <summary>
    /// 脱敏类
    /// </summary>
    public static class DataMaskingTool
    {
        /// <summary>
        /// 手机号
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string PhoneMasking(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length <= 4)
            {
                return phone == null ? "" : phone;
            }
            return phone.Replace(phone.Substring(0, phone.Length - 4), "*****");
        }


        public static string BankMasking(this string bankNum)
        {
            if (string.IsNullOrWhiteSpace(bankNum) || bankNum.Length <= 4)
            {
                return bankNum == null ? "" : bankNum;
            }
            return bankNum.Replace(bankNum.Substring(0, bankNum.Length - 4), "************");
        }
    }
}
