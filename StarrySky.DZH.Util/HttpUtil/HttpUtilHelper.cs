using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    /// <summary>
    /// Http帮助类（非爬虫用）
    /// </summary>
    public class HttpUtilHelper
    {
        #region 文件相关

        /// <summary>
        /// http请求文件到流中
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>returns</returns>
        public static byte[] GetFileByHttp(string url)
        {
            byte[] result = null;
            HttpWebRequest webRequest = null;
            ServicePointManager.DefaultConnectionLimit = 200;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.Timeout = 60000;
                webRequest.ServicePoint.Expect100Continue = false;

                // 响应流
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                var respStream = webResponse.GetResponseStream();  // 获取响应的字符串流
                result = respStream.StreamToByte();
            }
            catch (Exception ex)
            {
                throw new Exception("访问出错\r\n调用地址:" + url, ex);
            }

            return result;
        }

        /// <summary>
        /// 文件上传到别的地方
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filebytes">文件字节</param>
        /// <returns>http响应字符串</returns>
        public static string PostFile(string url, string fileName, byte[] filebytes)
        {
            var result = string.Empty;
            Stopwatch stopWatch = Stopwatch.StartNew();
            HttpWebRequest webRequest = null;
            ServicePointManager.DefaultConnectionLimit = 200;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "Post";
                CookieContainer cookieContainer = new CookieContainer();
                webRequest.CookieContainer = cookieContainer;
                webRequest.AllowAutoRedirect = true;
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                webRequest.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                webRequest.Timeout = 5 * 60000;
                webRequest.ServicePoint.Expect100Continue = false;
                byte[] startBoundaryBytes = Encoding.UTF8.GetBytes($"--{boundary}\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes($"\r\n--{boundary}--\r\n");

                // 请求头部信息
                StringBuilder boundaryBlock = new StringBuilder();
                /*
                StringBuilder boundaryBlock = new StringBuilder();
                boundaryBlock.Append($"--{timpstamp}\r\n");
                boundaryBlock.Append($"Content-Disposition: form-data; name=\"meta\";\r\n");
                boundaryBlock.Append($"Content-Type: application/json\r\n");
                boundaryBlock.Append($"\r\n");
                boundaryBlock.Append($"{reqjson.meta.PackJson()}\r\n");
                boundaryBlock.Append($"--{timpstamp}\r\n");
                boundaryBlock.Append($"Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\";\r\n");
                boundaryBlock.Append($"Content-Type: image/jpg\r\n");
                boundaryBlock.Append($"\r\n");
                */
                boundaryBlock.Append($"Content-Disposition: form-data; name=\"media\"; filename=\"{fileName}\";\r\n");
                boundaryBlock.Append($"Content-Type: application/octet-stream\r\n");
                boundaryBlock.Append($"\r\n");

                // 拿到http请求流，写入请求
                var requestStream = webRequest.GetRequestStream();
                requestStream.Write(startBoundaryBytes, 0, startBoundaryBytes.Length);
                var p1 = Encoding.UTF8.GetBytes(boundaryBlock.ToString());
                requestStream.Write(p1, 0, p1.Length);
                requestStream.Write(filebytes, 0, filebytes.Length);
                requestStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                requestStream.Close();

                // 响应流
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                result = GetResponseStreamToStr(webResponse); // 读取Response中的内容
            }
            catch (WebException err)
            {
                stopWatch.Stop();
                var elapsed = stopWatch.ElapsedMilliseconds;
                switch (err.Status)
                {
                    case WebExceptionStatus.Timeout:
                        result = "请求超时,耗时:" + elapsed + "，WebException:" + err;
                        break;
                    default:
                        result = "耗时:" + elapsed + "，WebException:" + err;
                        break;
                }

                if (webRequest != null)
                {
                    webRequest.Abort();
                    webRequest = null;
                    System.GC.Collect();
                }

                throw new Exception(result, err);
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                throw new Exception("访问出错\r\n调用地址:" + url, ex);
            }

            stopWatch.Stop();
            return result;
        }
        #endregion


        #region HttpWebRequest方式
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="isUseGzip"></param>
        /// <returns></returns>
        public static string GetResponseByPost(string url, string data, string contentType = "application/json", bool isUseGzip = false)
        {
            return GetHttpResponse(url, "Post", data, null, contentType, isUseGzip);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="isUseGzip"></param>
        /// <returns></returns>
        public static string GetResponseByGet(string url, string contentType = "application/json", bool isUseGzip = false)
        {
            return GetHttpResponse(url, "Get", null, null, contentType, isUseGzip);
        }

        /// <summary>
        /// Post提交请求，返回XML格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        public static string GetXmlByPost(string url, string data)
        {
            string contentType = "text/xml";
            return GetHttpResponse(url, "Post", data, null, contentType);
        }

        /// <summary>
        /// http请求，默认返回Json格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="postdata"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <param name="isUseGzip"></param>
        /// <returns></returns>
        private static string GetHttpResponse(string url, string method, string postdata, Dictionary<string, string> headers = null, string contentType = "application/json", bool isUseGzip = false)
        {
            method = method?.ToUpper() ?? "";
            var result = string.Empty;
            Stopwatch stopWatch = Stopwatch.StartNew();
            HttpWebRequest webRequest = null;
            ServicePointManager.DefaultConnectionLimit = 200;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);

                #region 请求头处理
                if (headers != null && headers.Any())
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Add(item.Key, item.Value);
                    }

                }
                #endregion

                switch (method)
                {
                    case "GET":
                        webRequest.Method = method;
                        break;
                    case "POST":
                        webRequest.Method = method;
                        if (!string.IsNullOrWhiteSpace(postdata))
                        {
                            var bytes = Encoding.UTF8.GetBytes(postdata);//Encoding.GetEncoding(encoding).GetBytes(data);
                            webRequest.ContentType = string.Format("{0};charset=UTF-8", contentType);//application/x-www-form-urlencoded
                            webRequest.ContentLength = bytes.Length;
                            //请求流
                            var requestStream = webRequest.GetRequestStream();
                            requestStream.Write(bytes, 0, bytes.Length);
                            requestStream.Close();
                        }
                        break;
                    default:
                        throw new Exception("无效的Http页面请求方法");
                }

                if (isUseGzip)
                {
                    webRequest.Headers.Add("Accept-Encoding", "gzip");
                    webRequest.Headers.Add("Content-Encoding", "gzip");
                }
                webRequest.Timeout = 5000;
                webRequest.ServicePoint.Expect100Continue = false;

                //响应流
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                result = GetResponseStreamToStr(webResponse);//读取Response中的内容
            }
            catch (WebException err)
            {
                stopWatch.Stop();
                var elapsed = stopWatch.ElapsedMilliseconds;
                switch (err.Status)
                {
                    case WebExceptionStatus.Timeout:
                        result = $"请求超时,耗时:{elapsed},网络异常:{err.Message}";
                        break;
                    default:
                        result = $"耗时:{elapsed}，状态码：{err.Status},网络异常:{err.Message}";
                        break;
                }

                if (webRequest != null)
                {
                    webRequest.Abort();
                    webRequest = null;
                    System.GC.Collect();
                }

                throw new Exception(result, err);
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                throw new Exception($"访问出错\r\n调用地址:{url}\r\n参数:{postdata}\r\n", ex);
            }

            stopWatch.Stop();
            return result;
        }


        /// <summary>
        /// 读取Response中的内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResponseStreamToStr(HttpWebResponse response)
        {
            try
            {
                var result = "";
                if (response != null)
                {
                    var respStream = response.GetResponseStream();   //获取响应的字符串流  
                    if (respStream != null)
                    {
                        if (response.ContentEncoding?.ToLower().Contains("gzip") ?? false)
                        {
                            //如果已经压缩，进行解压缩
                            GZipStream gzipStream = new GZipStream(respStream, CompressionMode.Decompress);
                            using (StreamReader streamReader = new StreamReader(gzipStream, Encoding.UTF8))
                            {
                                result = streamReader.ReadToEnd();//获取返回的信息
                            }
                        }
                        else
                        {
                            using (StreamReader streamReader = new StreamReader(respStream, Encoding.UTF8))
                            {
                                result = streamReader.ReadToEnd();//获取返回的信息
                            }
                        }
                        respStream.Close();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

    }
}
