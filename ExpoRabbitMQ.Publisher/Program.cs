using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.Text;

namespace ExpoRabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://tpivhhjj:vCIKKcmYXHXiF29djb8XCQgOodUrcf8X@lion.rmq.cloudamqp.com/tpivhhjj");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("logs", durable: true, type: ExchangeType.Fanout);

                    string message = GetMessage(args);
                    int i = 0;

                    while (i < 100)
                    {
                        var bodyByte = Encoding.UTF8.GetBytes($"{message}-{i}");
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        stopwatch.Start();

                        channel.BasicPublish("logs", routingKey: "", properties, body: bodyByte);


                        Console.WriteLine($"Mesajlar Gitti :{message}-{i}");
                        i++;

                    }
                    stopwatch.Stop();

                    Console.WriteLine($"Toplam Geçen (Süre): {stopwatch.Elapsed}");

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
