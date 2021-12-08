﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel
                                           // channel.QueueDeclare("hello-queue", false, false, false);

   var randomQueueName = "log-listener-queue";  // channel.QueueDeclare().QueueName;

   // channel.QueueDeclare(queue: randomQueueName,
   //                      durable: false,
   //                      exclusive: false,
   //                      autoDelete: false,
   //                      arguments: null);

   channel.QueueBind(queue: randomQueueName,
                     "logs-fanout",
                     routingKey: string.Empty,
                     arguments: null);


   channel.BasicQos(0, 1, false);
   var consumer = new EventingBasicConsumer(channel);
   channel.BasicConsume(
                        queue: randomQueueName,
                        autoAck: false,
                        consumer: consumer);
   Console.WriteLine("Loglar dinleniyor.");

   consumer.Received += (object sender, BasicDeliverEventArgs e) =>
   {
      var body = e.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Thread.Sleep(500);
      Console.WriteLine("Incoming Message : " + message);

      channel.BasicAck(e.DeliveryTag, false);
   };
   Console.ReadLine();
}