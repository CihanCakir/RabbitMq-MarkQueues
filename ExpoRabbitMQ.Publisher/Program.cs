using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.Text;

namespace ExpoRabbitMQ.Publisher
{
    class Program
    {

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
                    channel.ExchangeDeclare("direct-exchange", durable: true, type: ExchangeType.Fanout);

                    Array log_name_array = Enum.GetValues(typeof(LogNames));


                    int i = 0;

                    while (i < 100)
                    {

                        Random rnd = new Random();
                        LogNames log = (LogNames)log_name_array.GetValue(rnd.Next(log_name_array.Length));

                        var bodyByte = Encoding.UTF8.GetBytes($"log={log.ToString()}-{i}");
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        stopwatch.Start();

                        channel.BasicPublish("direct-exchange", routingKey: log.ToString(), properties, body: bodyByte);


                        Console.WriteLine($"Log mesajları gönderildi :{log.ToString()}-{i}");
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
