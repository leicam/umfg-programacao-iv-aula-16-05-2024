using RabbitMQ.Client;
using System.Text;

namespace umfg.publisher.console
{
    internal class Program
    {
        private const string C_CONNECTION_STRING = "amqps://biyvqons:GFBhQCGqFLv1BNy0gt1LbtyumvLBpBGm@jaragua.lmq.cloudamqp.com/biyvqons";
        private const string C_QUEUE = "umfg-programacao-iv-2024-teste";

        static void Main(string[] args)
        {
            try
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

                Console.WriteLine("Inicio do envio de mensagens");

                channel.BasicPublish(exchange: "",
                                        routingKey: C_QUEUE,
                                        basicProperties: null,
                                        body: Encoding.UTF8.GetBytes("Hello word!"));

                foreach (var i in Enumerable.Range(1, 10))
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    channel.BasicPublish(exchange: "",
                                        routingKey: C_QUEUE,
                                        basicProperties: null,
                                        body: Encoding.UTF8.GetBytes($"Mensagem {i}"));
                    Console.WriteLine($"Mensagem {i} enviada");
                }

                Console.WriteLine("Fim do envio de mensagens");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }
        }
    }
}