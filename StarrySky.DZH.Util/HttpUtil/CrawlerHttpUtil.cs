using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.HttpUtil
{
    /// <summary>
    /// 爬虫使用Http请求帮助类
    /// </summary>
   public  class CrawlerHttpUtil
    {
       
        public static readonly string JsonContentType = "application/json; charset=UTF-8";
        public static readonly string FormContentType = "application/x-www-form-urlencoded; charset=UTF-8";

        public static readonly string DefaultAccept = "application/json, text/javascript, */*; q=0.01";
        public static readonly string SpecialAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

        #region 此处模拟浏览器登录

        /// <summary>
        /// 模拟浏览器
        /// </summary>
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";

        /// <summary>
        /// 字符集格式
        /// </summary>
        private static readonly Encoding charset = Encoding.GetEncoding("utf-8");

        /// <summary>
        /// 总是接受验证方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }


        #region HTTP请求-禁止请求跳转

        /// <summary>
        /// HTTP请求方式（禁止请求的跳转）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="referer"></param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="tryCopunt"></param>
        /// <param name="keepAlive"></param>
        /// <param name="isSetAccept"></param>
        /// <returns></returns>
        private static HttpWebResponse HttpRequest(string url, IDictionary<string, string> parameters,
            Encoding charset, CookieContainer cookieContainer, string referer, string method,
            string contentType, int tryCopunt = 0, bool keepAlive = false, bool isSetAccept = false, string accept = null, Dictionary<string, string> headers = null)
        {
            tryCopunt = tryCopunt + 1;
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.DefaultConnectionLimit = 200;

            request = WebRequest.Create(url) as HttpWebRequest;
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = method;
            if (String.IsNullOrWhiteSpace(contentType))
            {
                request.ContentType = FormContentType;
            }
            else
            {
                request.ContentType = contentType;
            }
            request.UserAgent = DefaultUserAgent;
            if (keepAlive)
            {
                request.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            }
            if (isSetAccept)
            {
                if (!string.IsNullOrWhiteSpace(accept))
                {
                    request.Accept = accept;
                }
                else
                {
                    request.Accept = DefaultAccept;
                }
            }
            if (headers != null && headers.Count >= 1)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en;q=0.8");
            request.Timeout = 6000;
            request.ServicePoint.Expect100Continue = false;
            request.AllowAutoRedirect = false;//禁止跳转
            //request.Proxy = new WebProxy("127.0.0.1:8888", true); //本地调试开启
            if (!string.IsNullOrWhiteSpace(referer))
            {
                request.Referer = referer;//指定上一页
            }

            try
            {
                byte[] data = null;

                //如果需要POST数据     
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    data = charset.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                return request.GetResponse() as HttpWebResponse;
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
                return null;
            }
            catch (WebException webEx)
            {
                Thread.Sleep(500);
                request.Abort();
                request = null;
                System.GC.Collect();
                if (tryCopunt <= 3)
                {
                    return HttpRequest(url, parameters, charset, cookieContainer, referer, method, contentType, tryCopunt, keepAlive);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// HTTP-GET方式（禁止请求的跳转）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="referer"></param>
        /// <param name="contentType"></param>
        /// <param name="tryCopunt"></param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpWebResponse HttpGetRequest(string url, IDictionary<string, string> parameters,
             CookieContainer cookieContainer, string referer, string contentType = "", int tryCopunt = 0, bool keepAlive = false, string accept = null, bool isSetAccept = false, Dictionary<string, string> headers = null)
        {
            string method = "GET";
            return HttpRequest(url, parameters, charset, cookieContainer, referer, method, contentType, tryCopunt, keepAlive, isSetAccept, accept, headers);
        }

        /// <summary>
        /// HTTP-POST方式（禁止请求的跳转）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="referer"></param>
        /// <param name="contentType"></param>
        /// <param name="tryCopunt"></param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpWebResponse HttpPostRequest(string url, IDictionary<string, string> parameters,
             CookieContainer cookieContainer, string referer, string contentType = "", int tryCopunt = 0, bool keepAlive = false, bool isSetAccept = false, string accept = null, Dictionary<string, string> headers = null)
        {
            string method = "POST";
            return HttpRequest(url, parameters, charset, cookieContainer, referer, method, contentType, tryCopunt, keepAlive, isSetAccept, accept, headers);
        }

        #endregion


        /// <summary>
        /// Http 请求方法 当对方是JSON格式时
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static HttpWebResponse HttpPostJson(string url, CookieContainer cookieContainer, string Josn, int timeout = 5000,
            string Referer = "", bool keepAlive = false)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.DefaultConnectionLimit = 200;

            request = WebRequest.Create(url) as HttpWebRequest;
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            if (String.IsNullOrEmpty(Josn))
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            }
            else
            {
                request.ContentType = "application/json; charset=UTF-8";
            }
            request.UserAgent = DefaultUserAgent;
            if (keepAlive)
            {
                request.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            }
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en;q=0.8");
            request.Timeout = 6000;
            request.ServicePoint.Expect100Continue = false;

            if (!String.IsNullOrEmpty(Referer))
            {
                request.Referer = "http://gugong.228.com.cn/Home/Index";
            }

            try
            {
                byte[] data = null;

                data = charset.GetBytes(Josn);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException webEx)
            {
                request.Abort();
                request = null;
                System.GC.Collect();

                return null;
            }
        }

        /// <summary>
        /// Http 请求方法 当对方是JSON格式时
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="charset"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static HttpWebResponse HttpGetPostForm(string url, CookieContainer cookieContainer = null,
            IDictionary<string, string> parameters = null, string Method = "GET", int timeout = 5000,
            string Referer = "", bool keepAlive = false, string post_form = "")
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.DefaultConnectionLimit = 200;

            request = WebRequest.Create(url) as HttpWebRequest;
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = Method;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.UserAgent = DefaultUserAgent;
            if (keepAlive)
            {
                request.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            }
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en;q=0.8");
            request.Timeout = 6000;
            request.ServicePoint.Expect100Continue = false;
            if (!String.IsNullOrEmpty(Referer))
            {
                request.Referer = "http://gugong.228.com.cn/Home/Index";
            }

            try
            {
                byte[] data = null;

                //如果需要POST数据     
                if (!(parameters == null || parameters.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        }
                        i++;
                    }
                    data = charset.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                else if (!string.IsNullOrEmpty(post_form))
                {
                    data = charset.GetBytes(post_form);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException webEx)
            {
                request.Abort();
                request = null;
                System.GC.Collect();
                return null;
            }
        }
        #endregion

        #region 处理ResponseStream
        /// <summary>
        /// 读取Response中的内容 支持gzip的解析
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
