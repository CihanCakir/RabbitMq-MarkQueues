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
                    channel.QueueDeclare("cloud", false, false, false, null);
                    string message = "Cloud Firt Message";
                    var bodyByte = Encoding.UTF8.GetBytes(message);
                    int i = 0 ;
                    stopwatch.Start();

                    while (i < 1000)
                    {
                        channel.BasicPublish("", "cloud", null, body: bodyByte);
                        i++;
                    }
                    stopwatch.Stop();

                    Console.WriteLine("Mesajlar Gitti");
                    Console.WriteLine($"Toplam Geçen (Süre): {stopwatch.Elapsed}");

                }
                Console.WriteLine("Çıkış yapmak için Tıklayınız");
                Console.ReadLine();
            }
        }
    }
}
