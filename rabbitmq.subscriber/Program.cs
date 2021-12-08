using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

   var queueName = channel.QueueDeclare().QueueName;

   Dictionary<string, object> headers = new Dictionary<string, object>();
   headers.Add("format", "pdf");
   headers.Add("shape", "a4");
   headers.Add("x-match", "all");

   channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

   channel.BasicQos(0, 1, false);

   var consumer = new EventingBasicConsumer(channel);

   channel.BasicConsume(
                        queue: queueName,
                        autoAck: false,
                        consumer: consumer);
   Console.WriteLine("Loglar dinleniyor.");

   consumer.Received += (object sender, BasicDeliverEventArgs e) =>
   {
      var body = e.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Thread.Sleep(400);

      Console.WriteLine("Gelen Mesaj : " + message);

      channel.BasicAck(e.DeliveryTag, false);
   };
   Console.ReadLine();
}