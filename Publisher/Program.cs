using System;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {

            // Definindo uma conexão com um nó RabbitMQ em localhost
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            // Abrimos uma conexão com um nó RabbitMQ
            using var connection = factory.CreateConnection();

            // Criamos um canal onde vamos definir uma fila, uma mensagem e publicar a mensagem
            using var channel = connection.CreateModel();

            channel.QueueDeclare
            (
                queue: "saudacao_1",    // nome da fila
                durable: false,         // se igual a true a fila permanece ativa após o servidor ser reiniciado
                exclusive: false,       // se igual a true ela só pode ser acessada via conexão atual e são excluidas ao fechar a conexão
                autoDelete: false,      // se igual a true será deletada automaticamente após os consumidores usarem a fila
                arguments: null
            );

            // mensagem a ser enviada
            string message = "Welcome to RabbitMQ";

            // convertendo a mensagem para um array de bytes
            var body = Encoding.UTF8.GetBytes(message);

            // Publicando a mensagem
            channel.BasicPublish
            (
                exchange: "",
                routingKey: "saudacao_1",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"[x] Enviada: {message}");
        }
    }
}
