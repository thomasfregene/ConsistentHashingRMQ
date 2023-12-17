using RabbitMQ.Client;
using System.Text;

//PRODUCER

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("simplehashing", "x-consistent-hash");

var message = "Hash the routing and pass me on please";
var body = Encoding.UTF8.GetBytes(message);

var routingKey = "Hashxyz me!";

channel.BasicPublish("simplehashing", routingKey, null, body);

Console.WriteLine($"Sending message: {message}");