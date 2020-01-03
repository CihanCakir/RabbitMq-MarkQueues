using RabbitMQ.Client;
using System;
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
                    channel.ExchangeDeclare("topic-exchange", durable: true, type: ExchangeType.Topic);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));


                    int i = 0;

                    while (i < 100)
                    {

                        Random rnd = new Random();
                        LogNames log1 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log2 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        LogNames log3 = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));
                        string RoutingKey = $"{log1}.{log2}.{log3}";

                        var bodyByte = Encoding.UTF8.GetBytes($"log={log1.ToString()}-{log2.ToString()}-{log3.ToString()}");
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        stopwatch.Start();

                        channel.BasicPublish("topic-exchange", routingKey: RoutingKey, properties, body: bodyByte);


                        Console.WriteLine($"Log mesajları gönderildi => mesaj :{RoutingKey}-{i}");
                        i++;

                    }
                    stopwatch.Stop();

                    Console.WriteLine($"Toplam Geçen (Süre): {stopwatch.Elapsed.TotalSeconds}");

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
