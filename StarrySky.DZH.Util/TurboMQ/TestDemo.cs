
using StarrySky.DZH.Util.Extensions;
using SurpriseGamePoll.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace SurpriseGamePoll.Services.TurboMQ
{
    public class TestDemo
    {
        public void TestTag(string msg)
        {
            var ip = "";
            try
            {
                ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
                      .AddressList?.PackJson();//.FirstOrDefault(address => address.AddressFamily ==AddressFamily.InterNetwork)?.ToString();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                //LogBuilder.Info($"{Kernel.Environment.ToLower()},{ip},mq消费,msgBody={msg}", "新版MQ", "新版MQ");
            }
        }
    }
}
