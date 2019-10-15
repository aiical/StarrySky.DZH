using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    public class DateTimeHelper
    {
        
        /// <summary>
        /// 获取截止到当天24点的总秒数
        /// </summary>
        /// <returns></returns>
        public static double GetTodayRestSeconds()
        {
            return (DateTime.Today.AddDays(1) - DateTime.Now).TotalSeconds;
        }


        /// <summary>
        /// 将Unix时间戳转成DateTime
        /// </summary>
        /// <param name="timestamp">13位或10位时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(long timestamp)
        {
            var ticks = DateTime.Now.ToUniversalTime().Ticks - (timestamp * (timestamp.ToString().Length == 13 ? 10000 : 10000000) + 621355968000000000);
            TimeSpan ts = new TimeSpan(ticks);
            return DateTime.Now.Add(-ts);
        }
    }
}
