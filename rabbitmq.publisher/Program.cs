using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel

   channel.ExchangeDeclare(exchange: "logs-topic",
                           type: ExchangeType.Topic,
                           durable: false,
                           autoDelete: false,
                           arguments: null);

   Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
       Random rnd = new Random();
       LogNames log1 = (LogNames)rnd.Next(1, 4);
       LogNames log2 = (LogNames)rnd.Next(1, 4);
       LogNames log3 = (LogNames)rnd.Next(1, 4);

       var routeKey = $"{log1}.{log2}.{log3}";
       
       string message = $"log-type : {log1} - {log2} - {log3}";

       var messageBody = Encoding.UTF8.GetBytes(message);

       channel.BasicPublish(
                            exchange: "logs-topic",
                            routingKey: routeKey,
                            basicProperties: null,
                            body: messageBody);

       Console.WriteLine($"{message} - Log gönderildi. Çıkmak için bir tuşa basınız");
    });

   Console.ReadLine();
}

public enum LogNames
{
   Critical = 0,
   Error = 1,
   Info = 2,
   Warning = 3,
}