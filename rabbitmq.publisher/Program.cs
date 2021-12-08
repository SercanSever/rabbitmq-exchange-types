using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel

   channel.ExchangeDeclare(exchange: "logs-direct",
                           type: ExchangeType.Direct,
                           durable: false,
                           autoDelete: false,
                           arguments: null);

   Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
   {
      var routeKey = $"route*{x}";
      var queueName = $"direct-queue-{x}";
      channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
      channel.QueueBind(queue: queueName, exchange: "logs-direct", routingKey: routeKey, arguments: null);
   });

   Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
       LogNames log = (LogNames)new Random().Next(0, 4);

       string message = $"log-type : {log}";

       var messageBody = Encoding.UTF8.GetBytes(message);

       var routeKey = $"route*{log}";

       channel.BasicPublish(
                            exchange: "logs-direct",
                            routingKey: routeKey,
                            basicProperties: null,
                            body: messageBody);

       Console.WriteLine($"{message} - Mesajınız gönderildi. Çıkmak için bir tuşa basınız");
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