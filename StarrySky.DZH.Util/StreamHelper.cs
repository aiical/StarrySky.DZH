using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util
{
    /// <summary>
    /// 解决流复用的问题
    /// </summary>
    public static class StreamHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToByte(this Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
            //byte[] bytes = new byte[stream.Length]; //这里可能报错， http响应流不支持读
            //stream.Read(bytes, 0, bytes.Length);
            ////设置当前流的位置为流的开始
            //stream.Seek(0, SeekOrigin.Begin);
            //return bytes;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream ByteToStream(this byte[] bytes)
        {
            //将bytes转换为流
            Stream newStream = new MemoryStream(bytes);
            return newStream;
        }
    }
}
