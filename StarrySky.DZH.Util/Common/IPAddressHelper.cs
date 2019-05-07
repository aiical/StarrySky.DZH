using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.Common
{
    public class IPAddressHelper
    {
        #region 是否本地IP
        /// <summary>
        /// 是否本地IP
        /// </summary>
        /// <returns></returns>
        public static bool IsLocalIP()
        {
            if (System.Web.HttpContext.Current.Request.Url.Host.Equals("localhost")
                || System.Web.HttpContext.Current.Request.Url.Host.ToString().IndexOf("192").Equals(0)
                || System.Web.HttpContext.Current.Request.Url.Host.ToString().IndexOf("172").Equals(0))
                return true;
            else
                return false;
        }
        #endregion


        /// <summary>
        /// 获取服务器ip
        /// </summary>
        /// <returns></returns>
        public  static string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址                                                        //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];
            if (localaddr == null)
            {
                localaddr = IPAddress.Parse("127.0.0.1");
            }
            return localaddr.ToString();
        }


        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {

            string CustomerIP = "127.0.0.1";

            try
            {
                if (System.Web.HttpContext.Current == null
            || System.Web.HttpContext.Current.Request == null
            || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return CustomerIP;

                //CDN加速后取到的IP 
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


                if (!String.IsNullOrEmpty(CustomerIP))
                    return CustomerIP;

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                }

                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch
            {
            }

            return CustomerIP;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// webapi中获取
        /// </summary>
        /// <returns></returns>
        //private string GetClientIp1()
        //{
        //    var request = Request;
        //    var result = string.Empty;
        //    if (request.Properties.ContainsKey("MS_HttpContext"))
        //    {
        //        result = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        //    }
        //    //else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
        //    //{
        //    //    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        //    //    return prop.Address;
        //    //}
        //    else if (HttpContext.Current != null)
        //    {
        //        result = HttpContext.Current.Request.UserHostAddress;
        //    }

        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        result = result.Split(',')[0].Trim();
        //    }
        //    if (IsIpAddress(result)) //代理即是IP格式 
        //        return result;
        //    return string.Empty;

        //}


        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        private static bool IsIpAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
    }
}
