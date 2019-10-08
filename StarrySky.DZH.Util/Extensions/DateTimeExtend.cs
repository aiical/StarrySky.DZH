using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Extensions
{
    /// <summary>
    /// 日期扩展
    /// DateTime.Now 属性 获取一个 DateTime 对象，该对象设置为此计算机上的当前日期和时间，表示为本地时间。在中国就是北京时间。
    ///DateTime.UtcNow 属性 获取一个 DateTime 对象，该对象设置为此计算机上的当前日期和时间，表示为协调世界时(UTC)。通俗点就是格林威治时间的当前时间
    /// </summary>
    public static class DateTimeExtend
    {
        /// <summary>
        /// 时间戳(毫秒级别) 13位
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp_MilliSeconds(this DateTime datetime)
        {
            TimeSpan ts = datetime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ret;
        }

        /// <summary>
        /// 时间戳(秒级别) 10位
        /// </summary>
        /// <returns></returns>
        private static string GetTimeStamp_Seconds(this DateTime datetime)
        {
            TimeSpan ts = datetime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 转到固定格式 :dateFormat 0 返回 "yyyy/MM/dd HH:mm:ss", 1 返回"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dateFormat">0 返回 "yyyy/MM/dd HH:mm:ss" ,1 返回"yyyy-MM-dd HH:mm:ss"</param>
        /// <returns></returns>
        public static string ToFixFormat(this DateTime time, int dateFormat = 0)
        {
            if (dateFormat == 0)
            {
                return time.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
            else if (dateFormat == 1)
            {
                return time.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
            return time.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }


        /// <summary>
        /// 本周第一天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeek(this DateTime time)
        {
            int dayCount = (int)time.DayOfWeek - 1;
            if (dayCount >= 0)
            {
                return time.AddDays(-1 * dayCount);
            }
            else
            {
                return time.AddDays(-6);
            }
        }

    }
}
