using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    /// <summary>
    /// 验证文件类型
    /// </summary>
    public class ImgFileValidateHelper
    {
        /// <summary>
        /// 判断是否图片by文件头【推荐】
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        public static bool IsPicture(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fs);
                string fileClass;
                byte buffer;
                buffer = reader.ReadByte();
                fileClass = buffer.ToString();
                buffer = reader.ReadByte();
                fileClass += buffer.ToString();
                reader.Close();
                fs.Close();
                if (fileClass == FileExtension.JPG.GetHashCode().ToString() || fileClass == FileExtension.GIF.GetHashCode().ToString() || fileClass == FileExtension.BMP.GetHashCode().ToString() || fileClass == FileExtension.PNG.GetHashCode().ToString())

                //255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断文件是否为图片by流转化【不推荐】
        /// </summary>
        /// <param name="path">文件完整路径</param>
        /// <returns></returns>
        public static bool IsImage(string path)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }


        public enum FileExtension
        {
            JPG = 255216,
            GIF = 7173,
            PNG = 13780,
            BMP = 6677,
            SWF = 6787,
            RAR = 8297,
            ZIP = 8075,
            _7Z = 55122,
            VALIDFILE = 9999999
            // 255216 jpg;  

            // 7173 gif;  

            // 6677 bmp,  

            // 13780 png;  

            // 6787 swf  

            // 7790 exe dll,  

            // 8297 rar  

            // 8075 zip  

            // 55122 7z  

            // 6063 xml  

            // 6033 html  

            // 239187 aspx  

            // 117115 cs  

            // 119105 js  

            // 102100 txt  

            // 255254 sql   

        }
    }
}
