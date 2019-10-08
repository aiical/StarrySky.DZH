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
        /// 获取当天晚的秒数
        /// </summary>
        /// <returns></returns>
        public static double GetTodayExpireSeconds()
        {
            return (DateTime.Today.AddDays(1) - DateTime.Now).TotalSeconds;
        }
    }
}
