using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel
   var queueName = "direct-queue-Warning";
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

      Console.WriteLine("Incoming Message : " + message);

      // File.AppendAllText("log-Warning.txt",message + "\n");

      channel.BasicAck(e.DeliveryTag, false);
   };
   Console.ReadLine();
}