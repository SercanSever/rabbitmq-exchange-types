using System.Text;
using System.Text.Json;
using rabbitmq.shared;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel

   channel.ExchangeDeclare(exchange: "header-exchange",
                           type: ExchangeType.Headers,
                           durable: false,
                           autoDelete: false,
                           arguments: null);

   Dictionary<string, object> headers = new Dictionary<string, object>();

   headers.Add("format", "pdf");
   headers.Add("shape", "a4");

   var properties = channel.CreateBasicProperties();
   properties.Headers = headers;
   properties.Persistent = true; // messages are not deleted

   var product = new Product { Id = 1, Name = "Laptop", Price = 1000, Stock = 5 };
   var productJsonString = JsonSerializer.Serialize(product);

   channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

   Console.WriteLine("Mesaj Gönderildi");
   Console.ReadLine();
}

public enum LogNames
{
   Critical = 0,
   Error = 1,
   Info = 2,
   Warning = 3,
}