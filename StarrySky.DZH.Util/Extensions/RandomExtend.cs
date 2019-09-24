using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Extensions
{
    public static  class RandomExtend
    {
        public static double NextRandom(this Random r, double min, double max)
        {
            if (r == null) {
                r = new Random(DateTime.Now.Millisecond); //亦可Guid.NewGuid().GetHashCode()做种子
            }
            if (min == max)
            {
                return min;
            }
            else if (min > max)
            {
                min = max + min;
                max = min - max;
                min = min - max;
            }
            return r.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// 取随机数种子(性能稍弱)
        /// system.Security.Cryptography.RNGCryptoServiceProvider的类，它采用系统当前的硬件信息、进程信息、线程信息、系统启动时间和当前精确时间作为填充因子，通过更好的算法生成高质量的随机数，生成强随机字符串
        /// 由于RNGCryptoServiceProvider在生成期间需要查询上面提到的几种系统因子，所以性能稍弱于Random类
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
