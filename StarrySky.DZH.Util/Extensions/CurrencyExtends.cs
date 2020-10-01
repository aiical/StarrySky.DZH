using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Extensions
{
    /// <summary>
    /// 币别
    /// </summary>
    public enum CurrencyEnum
    {
        /// <summary>
        /// 人民币
        /// </summary>
        [Description("人民币")]
        CNY = 1,

        /// <summary>
        /// 英镑
        /// </summary>
        [Description("英镑")]
        GBP = 2,

        /// <summary>
        /// 港币
        /// </summary>
        [Description("港币")]
        HKD = 3,

        /// <summary>
        /// 美元
        /// </summary>
        [Description("美元")]
        USD = 4,

        /// <summary>
        /// 瑞士法郎
        /// </summary>
        [Description("瑞士法郎")]
        CHF = 5,

        /// <summary>
        /// 新加坡元
        /// </summary>
        [Description("新加坡元")]
        SGD = 6,

        /// <summary>
        /// 日元
        /// </summary>
        [Description("日元")]
        JPY = 7,

        /// <summary>
        /// 加拿大元
        /// </summary>
        [Description("加拿大元")]
        CAD = 8,

        /// <summary>
        /// 澳大利亚元
        /// </summary>
        [Description("澳大利亚元")]
        AUD = 9,

        /// <summary>
        /// 欧元
        /// </summary>
        [Description("欧元")]
        EUR = 10,

        /// <summary>
        /// 澳门元
        /// </summary>
        [Description("澳门元")]
        MOP = 11,

        /// <summary>
        /// 菲律宾披索
        /// </summary>
        [Description("菲律宾披索")]
        PHP = 12,

        /// <summary>
        /// 泰铢
        /// </summary>
        [Description("泰铢")]
        THB = 13,

        /// <summary>
        /// 新西兰元
        /// </summary>
        [Description("新西兰元")]
        NZD = 14,

        /// <summary>
        /// 韩币
        /// </summary>
        [Description("韩币")]
        KWP = 15,

        /// <summary>
        /// 卢布
        /// </summary>
        [Description("卢布")]
        RUB = 16,

        /// <summary>
        /// 印度尼西亚盾
        /// </summary>
        [Description("印度尼西亚盾")]
        IDR = 17
    }

    /// <summary>
    /// 币别扩展服务
    /// </summary>
    public static class CurrencyExtends
    {
        /// <summary>
        /// 转成通用货币符号
        /// </summary>
        /// <param name="currency">币别</param>
        /// <returns>字符</returns>
        public static string PackCurrencySymbols(this CurrencyEnum currency)
        {
            var symbol = "¥";
            if (!Enum.IsDefined(typeof(CurrencyEnum), currency))
            {
                return symbol;
            }

            switch (currency)
            {
                case CurrencyEnum.CNY:
                    symbol = "¥"; break;
                case CurrencyEnum.GBP:
                    symbol = "￡"; break;
                case CurrencyEnum.HKD:
                    symbol = "HK$"; break;
                case CurrencyEnum.USD:
                    symbol = "$"; break;
                case CurrencyEnum.CHF:
                    symbol = "CHF"; break;
                case CurrencyEnum.SGD:
                    symbol = "S$"; break;
                case CurrencyEnum.JPY:
                    symbol = "JPY￥"; break; // 日元：￥ 人民币：¥
                case CurrencyEnum.CAD:
                    symbol = "C$"; break;
                case CurrencyEnum.AUD:
                    symbol = "A$"; break;
                case CurrencyEnum.EUR:
                    symbol = "€"; break;
                case CurrencyEnum.MOP:
                    symbol = "MOP$"; break;
                case CurrencyEnum.PHP:
                    symbol = "₱"; break;
                case CurrencyEnum.THB:
                    symbol = "฿"; break;
                case CurrencyEnum.NZD:
                    symbol = "NZ$"; break;
                case CurrencyEnum.KWP:
                    symbol = "₩"; break;
                case CurrencyEnum.RUB:
                    symbol = "₽"; break;
                case CurrencyEnum.IDR:
                    symbol = "Rp"; break;
                default:
                    symbol = "¥"; break;
            }

            return symbol;
        }

        /// <summary>
        /// 转成金钱 千分位格式 “##,###,###.##”，无货币符号
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="currency">币别</param>
        /// <returns>字符</returns>
        public static string PackMoney(this object value, string currency)
        {
            Enum.TryParse(currency, true, out CurrencyEnum cury);
            return PackMoney(value, cury);
        }

        /// <summary>
        /// 【有货币符号】转成金钱 千分位格式 “¥##,###,###.##”
        /// 前面是货币符号
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="currency">币别</param>
        /// <returns>字符</returns>
        public static string PackMoneySymbol(this object value, string currency)
        {
            Enum.TryParse(currency, true, out CurrencyEnum cury);
            return PackMoneySymbol(value, cury);
        }

        /// <summary>
        /// 转成金钱 千分位格式 “##,###,###.##”，无货币符号
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="currency">币别</param>
        /// <returns>字符</returns>
        public static string PackMoney(this object value, CurrencyEnum currency = CurrencyEnum.CNY)
        {
            var amount = value.PackDecimal();
            if (amount == 0)
            {
                return string.Empty;
            }

            return string.Format("{0:N2}", amount); // “¥##,###,###.##”
        }

        /// <summary>
        /// 【有货币符号】转成金钱 千分位格式 “¥##,###,###.##”
        /// 前面是货币符号
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="currency">币别</param>
        /// <returns>字符</returns>
        public static string PackMoneySymbol(this object value, CurrencyEnum currency = CurrencyEnum.CNY)
        {
            var amount = value.PackDecimal();
            if (amount == 0)
            {
                return string.Empty;
            }

            return $"{currency.PackCurrencySymbols()}{string.Format("{0:N2}", amount)}"; // “¥##,###,###.##”
        }
    }
}
