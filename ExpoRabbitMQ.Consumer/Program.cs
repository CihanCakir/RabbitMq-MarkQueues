using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Consumer
{
    public enum LogNames
    {
        Critical,
        Error,
        Warning
    } 

    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
             factory.Uri = new Uri("amqp://tpivhhjj:vCIKKcmYXHXiF29djb8XCQgOodUrcf8X@lion.rmq.cloudamqp.com/tpivhhjj");


            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

                    
                    var queueName = channel.QueueDeclare("Obejct1",false,false,false,null).QueueName;

                    Dictionary<string, object> headers = new Dictionary<string, object>();
                    headers.Add("format", "pdf");
                    headers.Add("shape", "a4");
                    //publisherden gelen headerslerin hepsiyle uyumlu olması gerekiyorsa 
                // =>    headers.Add("x-match", "all");

                    // publisher headers da herhangi eşleşme yakalanmasını istiyorsak 
                    headers.Add("x-match", "any");

                    channel.QueueBind(queueName, "header-exchange", string.Empty, headers);
                   
                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume(queueName, false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine($"gelen mesaj: {message}");

                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    };
                    Console.WriteLine("Çıkış yapmak tıklayınız..");
                    Console.ReadLine();
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return args[0].ToString();
        }
        private static string GetLog(string[] args)
        {
            return args[1].ToString();
        }
    }
}