using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExpoRabbitMQ.Publisher
{
    class Program
    {
        // Critical.error.INfo Routing Key yada Info.Warning.Critical
        public enum LogNames
        {
            Critical = 1,
            Error = 2,
            Info = 3,
            Warning = 4

        }
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://tpivhhjj:vCIKKcmYXHXiF29djb8XCQgOodUrcf8X@lion.rmq.cloudamqp.com/tpivhhjj");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

                    var properties = channel.CreateBasicProperties();
                    Dictionary<string, object> headers = new Dictionary<string, object>();

                    headers.Add("ffsd", "pdf");
                    headers.Add("shape", "a4");
                    properties.Headers = headers;

                    Console.WriteLine("mesaj gönderildi..");
                    channel.BasicPublish("header-exchange", routingKey: string.Empty, properties, Encoding.UTF8.GetBytes("Header Mesaj"));
                }
                Console.WriteLine("Çıkış yapmak için Tıklayınız");
                Console.ReadLine();
            }
        }


        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}
