using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static string GetResponseByPost(string url, string param, string contentType = "application/json") {
            return GetHttpResponse(url,"Post",param,null,contentType);
        }

        public static string GetResponseByGet(string url, string param, string contentType = "application/json")
        {
            return GetHttpResponse(url, "Get", param, null, contentType);
        }

        /// <summary>
        /// Post提交请求，返回XML格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        public static string GetXmlByPost(string url, string param)
        {
            string contentType = "text/xml";
            return GetHttpResponse(url, "Post", param, null, contentType);
        }

        /// <summary>
        /// Post提交请求，默认返回Json格式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <param name="contextType"></param>
        /// <returns></returns>
        private static string GetHttpResponse(string url, string method, string param, Dictionary<string, string> headers=null,string contentType = "application/json")
        {
            var result = string.Empty;
            Stopwatch stopWatch = Stopwatch.StartNew();
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = method;

                #region 请求头处理
                if (headers != null && headers.Any())
                {
                    foreach (var item in headers)
                    {
                        webRequest.Headers.Add(item.Key, item.Value);
                    }

                }
                #endregion

                var data = Encoding.UTF8.GetBytes(param);//Encoding.GetEncoding(encoding).GetBytes(parms);
                webRequest.ContentType = contentType;//application/x-www-form-urlencoded
                webRequest.ContentLength = data.Length;
                webRequest.Timeout = 5000;
                //请求流
                var requestStream = webRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                //响应流
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                var responseStream = webResponse.GetResponseStream();
                if (responseStream != null)
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        //获取返回的信息
                        result = streamReader.ReadToEnd();
                    }
                    responseStream.Close();
                }
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
                throw new Exception("访问出错\r\n调用地址:" + url + "\r\n参数:" + param + "\r\n", ex);
            }

            stopWatch.Stop();
            return result;
        }

        #endregion
    }
}
