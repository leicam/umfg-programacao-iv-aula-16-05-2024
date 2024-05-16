using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace umfg.consumer.console
{
    internal class Program
    {
        private const string C_CONNECTION_STRING = "amqps://slwvfbfu:b3tRnEZ4DuF5RhkFPk6iBHay28no7qyv@prawn.rmq.cloudamqp.com/slwvfbfu";
        private const string C_QUEUE = "umfg-programacao-iv-2024-teste";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(C_CONNECTION_STRING)
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: C_QUEUE,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ConsumerReceived;

            channel.BasicConsume(queue: C_QUEUE, autoAck: true, consumer: consumer);
        }

        private static void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"[Nova mensagem | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {Encoding.UTF8.GetString(e.Body.ToArray())}");
        }
    }
}