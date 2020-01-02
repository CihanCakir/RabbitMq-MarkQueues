using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;

namespace ExpoRabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                Stopwatch stopwatch = new Stopwatch();

                var factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://tpivhhjj:vCIKKcmYXHXiF29djb8XCQgOodUrcf8X@lion.rmq.cloudamqp.com/tpivhhjj");
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("cloud", false, false, false, null);
                        var consumer = new EventingBasicConsumer(channel);
                        stopwatch.Start();

                        consumer.Received += (model, ea) =>
                         {
                             var message = Encoding.UTF8.GetString(ea.Body);

                             Console.WriteLine("Mesaj Alındı: " + message);
                         };

                        channel.BasicConsume("cloud", true, consumer);
                        stopwatch.Stop();
                        Console.WriteLine($"Toplam Geçen (Süre): {stopwatch.Elapsed}");

                        Console.ReadLine();


                    }
                }
            }
        }
    }
}
