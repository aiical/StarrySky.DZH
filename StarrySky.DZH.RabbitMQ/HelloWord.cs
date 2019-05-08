using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.RabbitMQ
{
    /// <summary>
    /// nuget RabbitMQ.Client
    /// </summary>
    public class HelloWord
    {
        public static void HelloWordClient()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "guest";
            factory.Password = "guest";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("myChannel", false, false, false, null);
                    string message = "Hello World";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "hello", null, body);
                    Console.WriteLine(" set {0}", message);
                }
            }
        }


        public static void HelloWordServer()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "yy";
            factory.Password = "hello!";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("myChannel", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("myChannel", true, consumer);

                    Console.WriteLine(" waiting for message.");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("Received {0}", message);

                    }
                }
            }
        }
    }

}
