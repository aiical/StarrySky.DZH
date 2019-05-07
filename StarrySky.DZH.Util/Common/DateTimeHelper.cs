using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMaizuo.Common
{
    public class DateTimeHelper
    {

        /// <summary>
        /// 获取当前日期到GMT的毫秒数
        /// </summary>
        /// <returns></returns>
        public static string GetMillisecondsSpan()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ret;
        }
    }
}
