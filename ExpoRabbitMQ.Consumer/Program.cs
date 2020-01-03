﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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
                    channel.ExchangeDeclare("topic-exchange", durable: true, type: ExchangeType.Topic);

                    var queueName = channel.QueueDeclare().QueueName;

                    string routingKey = "Info.*.Warning";

                    channel.QueueBind(queue: queueName, exchange: "topic-exchange", routingKey: routingKey);
       
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);

                    Console.WriteLine($"{routingKey} logları bekliyorum....");

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume(queueName, false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var log = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine("log alındı:" + log);

                        int time = int.Parse(GetMessage(args));
                        Thread.Sleep(time);
                        File.AppendAllText("Info_Blank_Warning.txt",log +"\n");
                        Console.WriteLine("loglama bitti");

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
    }
}