using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//CONSUMER

var factory = new ConnectionFactory { HostName= "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("simplehashing", "x-consistent-hash");

channel.QueueDeclare("letterbox3");
channel.QueueDeclare("letterbox4");

//routing key rep the hash space
channel.QueueBind("letterbox3", "simplehashing", "1");
channel.QueueBind("letterbox4", "simplehashing", "3");

var consumer1 = new EventingBasicConsumer(channel);

consumer1.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue1 Received message: {message}");
};

channel.BasicConsume("letterbox3", autoAck: true, consumer: consumer1);

var consumer2 = new EventingBasicConsumer(channel);

consumer2.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue2 Received message: {message}");
};

channel.BasicConsume("letterbox4", autoAck: true, consumer: consumer2);

Console.WriteLine("Consuming");
Console.ReadKey();