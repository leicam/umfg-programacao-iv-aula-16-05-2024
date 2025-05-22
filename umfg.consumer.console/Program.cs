using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace umfg.consumer.console
{
    internal class Program
    {
        private const string C_CONNECTION_STRING = "amqps://biyvqons:GFBhQCGqFLv1BNy0gt1LbtyumvLBpBGm@jaragua.lmq.cloudamqp.com/biyvqons";
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

            consumer.Received += ConsumerReceived; //delegado

            channel.BasicConsume(queue: C_QUEUE, autoAck: true, consumer: consumer);

            Console.WriteLine("Pressione [enter] para sair.");
            Console.ReadLine();
        }

        private static void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($"[Nova mensagem | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] {Encoding.UTF8.GetString(e.Body.ToArray())}");
        }
    }
}