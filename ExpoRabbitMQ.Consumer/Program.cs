using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

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
                        channel.QueueDeclare("task_queue", true, false, false, null);
                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        Console.WriteLine("...Mesajlar Bekleniyor");
                        var consumer = new EventingBasicConsumer(channel);

                        channel.BasicConsume("task_queue", autoAck: false, consumer);
                        stopwatch.Start();



                        consumer.Received += (model, ea) =>
                        {

                            var message = Encoding.UTF8.GetString(ea.Body);

                            Console.WriteLine("Mesaj Alındı: " + message);
                            int time = int.Parse(GetMessage(args));
                            Thread.Sleep(time);
                            Console.WriteLine("Mesaj işlendi..");
                            channel.BasicAck(ea.DeliveryTag, false);

                        };
                        stopwatch.Stop();

                        Console.WriteLine($"Toplam Geçen (Süre): {stopwatch.Elapsed}");



                    }
                    Console.ReadLine();

                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
    }
}
