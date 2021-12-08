using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rwxubjos:j1HoQTDNTJ4j-Qv5UKzqfeX1uHdwIMKt@hornet.rmq.cloudamqp.com/rwxubjos");

using (var connection = factory.CreateConnection()) //connection
{
   var channel = connection.CreateModel(); //channel
   // channel.QueueDeclare("hello-queue", false, false, false);

   channel.BasicQos(0, 1, false);
   var consumer = new EventingBasicConsumer(channel);
   channel.BasicConsume(
                        queue: "hello-queue",
                        autoAck: false,
                        consumer: consumer);



   consumer.Received += (object sender, BasicDeliverEventArgs e) =>
   {
      var body = e.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Thread.Sleep(1500);
      Console.WriteLine("Incoming Message : " + message);

      channel.BasicAck(e.DeliveryTag, false);
   };
   Console.ReadLine();
}