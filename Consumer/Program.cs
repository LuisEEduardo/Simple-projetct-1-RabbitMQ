using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();

            // Criar o canal na conexão para operar
            using var channel = connection.CreateModel();

            // Declaramos a fila a partir da qual vamos consumir as mensagens
            channel.QueueDeclare
            (
                queue: "saudacao_1",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Solicita a entrega das mensagens de forma assíncrona e fornece um retorno de chamada
            var consumer = new EventingBasicConsumer(channel);

            // Recebe a mensagem da fila, converte para string e imprime o console da mensagem
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Recebida: {message}");
            };

            // Indica o consumo da mensagem
            channel.BasicConsume
            (
                queue: "saudacao_1",
                autoAck: true,
                consumer: consumer
            );

        }
    }
}
