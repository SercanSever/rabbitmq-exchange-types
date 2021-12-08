using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel
   channel.QueueDeclare(
                        queue: "hello-queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false);
   Enumerable.Range(1,50).ToList().ForEach(x =>
   {

      string message = $"Message {x} !";  //byte dizisi olarak gider.
      var messageBody = Encoding.UTF8.GetBytes(message);
      channel.BasicPublish(
                           exchange: string.Empty,
                           routingKey: "hello-queue",
                           basicProperties: null,
                           body: messageBody);

      Console.WriteLine($"{message} - Mesajınız gönderildi. Çıkmak için bir tuşa basınız");
   });

   Console.ReadLine();
}