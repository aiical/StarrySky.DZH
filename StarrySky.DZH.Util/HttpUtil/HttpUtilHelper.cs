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
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

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
                        result = "请求超时,耗时:" + elapsed + "，WebException:" + err;
                        break;
                    default:
                        result = "耗时:" + elapsed + "，WebException:" + err;
                        break;
                }
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                throw new Exception("访问出错\r\n调用地址:" + url + "\r\n参数:" + postdata + "\r\n", ex);
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
