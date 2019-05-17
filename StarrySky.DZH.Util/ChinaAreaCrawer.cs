using HtmlAgilityPack;
using StarrySky.DZH.Util.DataConvert;
using StarrySky.DZH.Util.HttpUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util
{
    public class ChinaAreaCrawer
    {
        //国家民政部官网http://www.mca.gov.cn/article/sj/xzqh/2019/
        public static string url = "http://www.mca.gov.cn/article/sj/xzqh/2019/201901-06/201904301706.html";

        public static void CreateJson()
        {
            List<AreaConfig> result = new List<AreaConfig>();
            Dictionary<string, Dictionary<string, string>> resultJSON = new Dictionary<string, Dictionary<string, string>>();
            var response = CrawlerHttpUtil.HttpGetRequest(url, null, null, null);
            var response_Str = CrawlerHttpUtil.GetResponseStreamToStr(response);
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response_Str);
                HtmlNode rootNode = doc.DocumentNode;
                var nodes = rootNode.SelectNodes("//tr[@height='19']");
                if (nodes != null && nodes.Any())
                {
                    HtmlAttribute att = null;
                    foreach (var elem in nodes)
                    {
                        var s = elem.SelectNodes(".//td[@class='xl7032454']");
                        result.Add(new AreaConfig()
                        {
                            AreaCode = s.First().InnerText,
                            AreaName = s.Last().InnerText
                        });
                    }
                }
                var provList = result.Where(p => p.AreaCode.Substring(2) == "0000").ToList();
                var provDic = provList.ToDictionary(p => p.AreaCode, p => p.AreaName);//110000
                var city = result.Where(p => p.AreaCode.Substring(2) != "0000" && p.AreaCode.Substring(4) == "00").ToList();
                var cityDic = city.ToDictionary(p => p.AreaCode, p => p.AreaName);
                //json 格式
                //  {
                //     86:{},
                //     110000:{},
                //     120000:{},
                //   }
                //结束
                resultJSON.Add("86", provDic);
                foreach (var item in provList)
                {
                    resultJSON.Add(item.AreaCode,city.Where(p => p.AreaCode.Substring(0, 2) == item.AreaCode.Substring(0, 2)).ToList().ToDictionary(p => p.AreaCode, p => p.AreaName)
);
                }

                var ss = resultJSON.PackJson();

            }
            catch (Exception ex)
            {
            }
            finally
            {
            }


        }


    }

    public class AreaConfig
    {
        public string AreaCode { get; set; }

        public string AreaName { get; set; }
    }

    public class AreaJson
    {
        public string Key { get; set; }

        public List<AreaConfig> Value { get; set; }
    }
}
