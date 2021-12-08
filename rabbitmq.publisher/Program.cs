using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel

   channel.ExchangeDeclare(exchange: "logs-fanout",
                           type: ExchangeType.Fanout,
                           durable: false,
                           autoDelete: false,
                           arguments: null);

   Enumerable.Range(1, 50).ToList().ForEach(x =>
    {

       string message = $"Message {x} !";  //byte dizisi olarak gider.
       var messageBody = Encoding.UTF8.GetBytes(message);
       channel.BasicPublish(
                            exchange: "logs-fanout",
                            routingKey: string.Empty,
                            basicProperties: null,
                            body: messageBody);

       Console.WriteLine($"{message} - Mesajınız gönderildi. Çıkmak için bir tuşa basınız");
    });

   Console.ReadLine();
}