using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StarrySky.DZH.Util
{
    public class CompressImageHelper
    {
        #region copy 未整理，但是可使用

        /// <summary>
        /// 压缩图片并保存
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        public void GetReducedImage1(string originalImagePath, string thumbnailPath)
        {
            int minSize = 1024;
            Graphics draw = null;

            System.Drawing.Image resourceImage = System.Drawing.Image.FromFile(originalImagePath);

            double percent = 0.4;
            int imageWidth = Convert.ToInt32(resourceImage.Width);
            int imageHeight = Convert.ToInt32(resourceImage.Height);
            if (imageWidth > imageHeight)
            {
                if (imageWidth > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageWidth;
                    imageWidth = minSize;
                    imageHeight = (int)(imageHeight * percent);
                }
            }
            else
            {
                if (imageHeight > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageHeight;
                    imageHeight = minSize;
                    imageWidth = (int)(imageWidth * percent);
                }
            }

            // 新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(imageWidth, imageHeight);
            System.Drawing.Image bitmap2 = new System.Drawing.Bitmap(imageWidth, imageHeight);

            // 新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            try
            {
                // 设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                // 设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // 清空画布并以透明背景色(白色)填充
                g.Clear(System.Drawing.Color.White);

                // 在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(resourceImage, new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight));

                // 用新建立的image对象拷贝bitmap对象 让g对象可以释放资源
                draw = Graphics.FromImage(bitmap2);
                draw.DrawImage(bitmap, 0, 0);

                // 设置缩略图编码格式
                ImageCodecInfo ici = null;
                if (Path.GetExtension(originalImagePath).Equals(".png") || Path.GetExtension(originalImagePath).Equals(".PNG"))
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }
                else if (Path.GetExtension(originalImagePath).Equals(".gif") || Path.GetExtension(originalImagePath).Equals(".GIF"))
                {
                    ici = GetImageCoderInfo("image/gif");
                }
                else
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }

                // 设置压缩率
                long ratio = 90L; // 压缩为原图90%的质量

                System.Drawing.Imaging.Encoder ecd = System.Drawing.Imaging.Encoder.Quality;

                resourceImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                draw.Dispose();

                // 保存调整在这里即可
                using (EncoderParameters eptS = new EncoderParameters(1))
                {
                    using (EncoderParameter ept = new EncoderParameter(ecd, ratio))
                    {
                        eptS.Param[0] = ept;
                        bitmap2.Save(thumbnailPath, ici, eptS);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(originalImagePath);
            }
            finally
            {
                if (resourceImage != null)
                {
                    resourceImage.Dispose();
                }

                if (bitmap != null)
                {
                    bitmap.Dispose();
                }

                if (g != null)
                {
                    g.Dispose();
                }

                if (bitmap2 != null)
                {
                    bitmap2.Dispose();
                }

                if (draw != null)
                {
                    draw.Dispose();
                }
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="stream">stream</param>
        /// <returns>byte</returns>
        public byte[] GetReducedImage(string fileName, Stream stream)
        {
            int minSize = 1024;
            Graphics draw = null;

            System.Drawing.Image resourceImage = System.Drawing.Image.FromStream(stream);

            double percent = 0.4;
            int imageWidth = Convert.ToInt32(resourceImage.Width);
            int imageHeight = Convert.ToInt32(resourceImage.Height);
            if (imageWidth > imageHeight)
            {
                if (imageWidth > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageWidth;
                    imageWidth = minSize;
                    imageHeight = (int)(imageHeight * percent);
                }
            }
            else
            {
                if (imageHeight > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageHeight;
                    imageHeight = minSize;
                    imageWidth = (int)(imageWidth * percent);
                }
            }

            // 新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(imageWidth, imageHeight);
            System.Drawing.Image bitmap2 = new System.Drawing.Bitmap(imageWidth, imageHeight);

            // 新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            try
            {
                // 设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                // 设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // 清空画布并以透明背景色(白色)填充
                g.Clear(System.Drawing.Color.White);

                // 在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(resourceImage, new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight));

                // 用新建立的image对象拷贝bitmap对象 让g对象可以释放资源
                draw = Graphics.FromImage(bitmap2);
                draw.DrawImage(bitmap, 0, 0);

                // 设置缩略图编码格式
                ImageCodecInfo ici = null;
                if (fileName.ToUpper().Contains(".PNG"))
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }
                else if (fileName.ToUpper().Contains(".GIF"))
                {
                    ici = GetImageCoderInfo("image/gif");
                }
                else
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }

                // 设置压缩率
                long ratio = 90L; // 压缩为原图90%的质量

                System.Drawing.Imaging.Encoder ecd = System.Drawing.Imaging.Encoder.Quality;

                resourceImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                draw.Dispose();

                // 保存调整在这里即可
                var respStream = new MemoryStream();
                using (EncoderParameters eptS = new EncoderParameters(1))
                {
                    using (EncoderParameter ept = new EncoderParameter(ecd, ratio))
                    {
                        eptS.Param[0] = ept;
                        bitmap2.Save(respStream, ici, eptS);
                    }
                }

                return respStream.StreamToByte();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (resourceImage != null)
                {
                    resourceImage.Dispose();
                }

                if (bitmap != null)
                {
                    bitmap.Dispose();
                }

                if (g != null)
                {
                    g.Dispose();
                }

                if (bitmap2 != null)
                {
                    bitmap2.Dispose();
                }

                if (draw != null)
                {
                    draw.Dispose();
                }
            }
        }


        #endregion

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片地址</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <param name="sfsc">是否是第一次调用</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int flag = 90, int size = 300, bool sfsc = true)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            FileInfo firstFileInfo = new FileInfo(sFile);
            if (sfsc == true && firstFileInfo.Length < size * 1024)
            {
                firstFileInfo.CopyTo(dFile);
                return true;
            }

            int dHeight = iSource.Height / 2;
            int dWidth = iSource.Width / 2;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                    FileInfo fi = new FileInfo(dFile);
                    if (fi.Length > 1024 * size)
                    {
                        flag = flag - 10;
                        CompressImage(sFile, dFile, flag, size, false);
                    }
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        /// <summary>
        /// 压缩图片并保存
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        public void GetReducedImage(string originalImagePath, string thumbnailPath)
        {
            int minSize = 1024;
            Graphics draw = null;

            System.Drawing.Image resourceImage = System.Drawing.Image.FromFile(originalImagePath);

            double percent = 0.4;
            int imageWidth = Convert.ToInt32(resourceImage.Width);
            int imageHeight = Convert.ToInt32(resourceImage.Height);
            if (imageWidth > imageHeight)
            {
                if (imageWidth > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageWidth;
                    imageWidth = minSize;
                    imageHeight = (int)(imageHeight * percent);
                }
            }
            else
            {
                if (imageHeight > minSize)
                {
                    percent = Convert.ToDouble(minSize) / imageHeight;
                    imageHeight = minSize;
                    imageWidth = (int)(imageWidth * percent);
                }
            }

            // 新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(imageWidth, imageHeight);
            System.Drawing.Image bitmap2 = new System.Drawing.Bitmap(imageWidth, imageHeight);

            // 新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            try
            {
                // 设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                // 设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // 清空画布并以透明背景色(白色)填充
                g.Clear(System.Drawing.Color.White);

                // 在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(resourceImage, new System.Drawing.Rectangle(0, 0, imageWidth, imageHeight));

                // 用新建立的image对象拷贝bitmap对象 让g对象可以释放资源
                draw = Graphics.FromImage(bitmap2);
                draw.DrawImage(bitmap, 0, 0);

                // 设置缩略图编码格式
                ImageCodecInfo ici = null;
                if (Path.GetExtension(originalImagePath).Equals(".png") || Path.GetExtension(originalImagePath).Equals(".PNG"))
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }
                else if (Path.GetExtension(originalImagePath).Equals(".gif") || Path.GetExtension(originalImagePath).Equals(".GIF"))
                {
                    ici = GetImageCoderInfo("image/gif");
                }
                else
                {
                    ici = GetImageCoderInfo("image/jpeg");
                }

                // 设置压缩率
                long ratio = 90L; // 压缩为原图90%的质量

                System.Drawing.Imaging.Encoder ecd = System.Drawing.Imaging.Encoder.Quality;

                resourceImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                draw.Dispose();

                // 保存调整在这里即可
                using (EncoderParameters eptS = new EncoderParameters(1))
                {
                    using (EncoderParameter ept = new EncoderParameter(ecd, ratio))
                    {
                        eptS.Param[0] = ept;
                        bitmap2.Save(thumbnailPath, ici, eptS);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(originalImagePath);
            }
            finally
            {
                if (resourceImage != null)
                {
                    resourceImage.Dispose();
                }

                if (bitmap != null)
                {
                    bitmap.Dispose();
                }

                if (g != null)
                {
                    g.Dispose();
                }

                if (bitmap2 != null)
                {
                    bitmap2.Dispose();
                }

                if (draw != null)
                {
                    draw.Dispose();
                }
            }
        }


        /// <summary>
        /// 获取图片编码
        /// </summary>
        /// <param name="coderType">编码格式：image/png、image/jpeg等</param>
        /// <returns>图片编码</returns>
        private ImageCodecInfo GetImageCoderInfo(string coderType)
        {
            ImageCodecInfo[] iciS = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo retIci = null;
            foreach (ImageCodecInfo ici in iciS)
            {
                if (ici.MimeType.Equals(coderType))
                {
                    retIci = ici;
                }
            }

            return retIci;
        }
    }
}
