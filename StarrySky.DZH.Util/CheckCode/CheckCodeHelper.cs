using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.CheckCode
{
    /// <summary>
    /// 验证码
    /// <para>Author:丁振华</para>
    /// </summary>
    public class CheckCodeHelper
    {

        private static string validateCodeType = "0";

        /// <summary>
        /// 验证码字符个数
        /// </summary>
        private static int _validateCodeLength;

        /// <summary>
        /// 验证码的字符集，去掉了一些容易混淆的字符
        /// </summary>
        private static char[] character = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };

        public static string GenerateCode(int length = 4)
        {
            _validateCodeLength = length;
            char code;
            string ValidateCode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < _validateCodeLength; i++)
            {
                code = character[random.Next(character.Length)];
                // 要求全为数字或字母
                if (validateCodeType == "1")
                {
                    if ((int)code < 48 || (int)code > 57)
                    {
                        i--;
                        continue;
                    }
                }
                else if (validateCodeType == "2")
                {
                    if ((int)code < 65 || (int)code > 90)
                    {
                        i--;
                        continue;
                    }
                }
                ValidateCode += code;
            }

            return ValidateCode;
        }
        public static void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 15.0 + 40)), 23);
            Graphics graphics = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                graphics.Clear(System.Drawing.Color.White);
                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Silver), x1, y1, x2, y2);
                }
                System.Drawing.Font font = new System.Drawing.Font("Arial", 14, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Color.Blue, System.Drawing.Color.DarkRed, 1.2f, true);
                int cySpace = 16;
                for (int i = 0; i < _validateCodeLength; i++)
                {
                    graphics.DrawString(checkCode.Substring(i, 1), font, brush, (i + 1) * cySpace, 1);
                }
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //MemoryStream ms = new MemoryStream();
                image.Save(@"D:\Log\validateCode.png", System.Drawing.Imaging.ImageFormat.Png);

            }
            finally
            {
                graphics.Dispose();
                image.Dispose();
            }
        }
    }
}