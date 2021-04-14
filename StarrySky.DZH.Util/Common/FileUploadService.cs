using StarrySky.DZH.Util.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    public class FileUploadService
    {
        #region 自有文件上传

        /// <summary>
        /// ofd 文件预览网络图像下载并保存
        /// </summary>
        /// <param name="httpUrl">httpUrl</param>
        /// <param name="fileName">fileName</param>
        /// <returns>returns</returns>
        public FileInfoDto Upload_OFD_PreviewUrl(string httpUrl, string fileName)
        {
            var bytes = HttpUtilHelper.GetFileByHttp(httpUrl);
            var basePath = "/var/www/uploads/ofd";
            return UploadToLocal(bytes.ByteToStream(), fileName, basePath).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="sourceFileName">sourceFileName</param>
        /// <param name="basePath">basePath</param>
        /// <returns>returns</returns>
        public async Task<FileInfoDto> UploadToLocal(Stream stream, string sourceFileName, string basePath)
        {
            var result = new FileInfoDto();

            basePath = basePath ?? Path.Combine(Path.GetTempPath(), "UploadTemporary");

            string directory = Path.Combine(basePath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);  // 会从当前项目盘符找目录
            }

            string newfileName = string.Concat(Guid.NewGuid(), GetExtension(sourceFileName));
            string fullPath = Path.Combine(directory, newfileName);

            // CreateNew 指定操作系统应创建新文件，如果文件存在则引发异常。
            // Create 指定操作系统创建新文件，如果文件已存在则覆盖之。
            using (var targetStream = new FileStream(fullPath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(targetStream);
            }

            result = new FileInfoDto()
            {
                Name = newfileName,
                Path = fullPath,
                FileRawName = sourceFileName.ContainsOneOfSpecial(":,\\,/") ? Path.GetFileName(sourceFileName) : sourceFileName
            };

            return result;
        }

        private static string GetExtension(string fileName)
        {
            string[] array = fileName.Split('.');
            if (array.Length < 2)
            {
                return string.Empty;
            }

            return "." + array[array.Length - 1];
        }
        #endregion

        #region 调用api上传

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="fileStream">文件流</param>
        /// <returns>YZFileResponse</returns>
        public FileInfoDto UploadFile(string filename, Stream fileStream)
        {
            var result = new FileInfoDto();
            try
            {
                string url = string.Empty;
                var respRaw = HttpUtilHelper.PostFile(url, filename, fileStream.StreamToByte());
                result = respRaw.PackJsonObject<FileInfoDto>();
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 获取文件字节流
        /// </summary>
        /// <param name="filePath">传document中存的地址</param>
        /// <returns>returns</returns>
        public byte[] GetFileByDocumentPath(string filePath)
        {
            byte[] bytes = new byte[0];
            if (filePath.ToLower().StartsWith("http"))
            {
                bytes = HttpUtilHelper.GetFileByHttp(filePath);
            }
            else
            {
                bytes = GetLocalFileByStream(filePath);
            }

            return bytes;
        }

        /// <summary>
        /// 为了避免流复用和不关闭问题，  统一返回byte,可自行.ByteToStream()
        /// </summary>
        /// <param name="localPath">localPath</param>
        /// <returns>统一返回byte</returns>
        public byte[] GetLocalFileByStream(string localPath)
        {
            // 将图片以文件流的形式进行保存
            using (FileStream fs = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                return fs.StreamToByte();
            }
        }
    }

    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfoDto
    {
        /// <summary>
        /// 文件原名
        /// </summary>
        public string FileRawName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extname { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
