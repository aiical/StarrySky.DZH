using StarrySky.DZH.Util.Extensions;
using System;


namespace SurpriseGamePoll.Services.TurboMQBusiness
{
    public class TestDemo
    {
        public void TestTag(string msg)
        {
            string myEnvironmentValue = Environment.GetEnvironmentVariable("test_qa", EnvironmentVariableTarget.Process);
            //LogBuilder.Info($"Process{myEnvironmentValue??""}", "新版MQ", "myEnvironmentValue");
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
