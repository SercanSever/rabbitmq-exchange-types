using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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

   var queueName = channel.QueueDeclare().QueueName;

   var routeKey = "*.Error.*";

   channel.QueueBind(queueName, "logs-topic", routeKey, arguments: null);

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

      Console.WriteLine("Log : " + message);

      // File.AppendAllText("log-Warning.txt",message + "\n");

      channel.BasicAck(e.DeliveryTag, false);
   };
   Console.ReadLine();
}