using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StarrySky.DZH.Util.Encrypt
{
    public class Sha256WithRSAHelper
    {
        /// <summary>
        /// SHA256 with RSA
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Sign(string message)
        {
            try
            {
                //LogBuilder.Info($"请求参数：{message}", "Sign", "Sign");
                // NOTE： 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----
                //        亦不包括结尾的-----END PRIVATE KEY-----
                //string privateKey = "MIIEvgI...........JMBw/BYPH";
                //byte[] keyData = Convert.FromBase64String(privateKey);


                using (RSACryptoServiceProvider sha256 = new RSACryptoServiceProvider())
                {
                    var privateKey = GetPrivateKey();  //获取私钥
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                    sha256.FromXmlString(privateKey);
                    byte[] inArray = sha256.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
                    string sign = Convert.ToBase64String(inArray);
                    return sign;
                }

                //CngKey.Import在某些环境可能报错
                //using (CngKey cngKey = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
                //using (RSACng rsa = new RSACng(cngKey))
                //{
                //    byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                //    return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
                //}
            }
            catch (ArgumentNullException argex)
            {
                
            }
            catch (PlatformNotSupportedException platex)
            {
                
            }
            catch (CryptographicException cryex)
            {
                
            }
            catch (Exception ex)
            {
                
                return "";
            }
            return "";
        }

        /// <summary>
        /// 获取私钥
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="password">文件秘钥</param>
        /// <returns></returns>
        public static string GetPrivateKey()
        {
            try
            {
                var path = GetFilePath("~/c1e2r4t", "apiclient_cert.p12");
                X509Certificate2 cert = new X509Certificate2(path, "1579811001", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                return cert.PrivateKey.ToXmlString(true);
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public static string GetFilePath(string path, string fileName)
        {
            try
            {
                return string.Concat(HttpContext.Current.Server.MapPath(path), $"/{fileName}");
            }
            catch (Exception ex)
            {
                //LogBuilder.Warn($"参数：{HttpContext.Current?.PackJson() ?? ""}，ex:{ex.ToString()}", "CommonServerExtend", "GetFilePath");
                return string.Concat(System.Web.Hosting.HostingEnvironment.MapPath(path), $"/{fileName}");
            }
        }

    }
}
