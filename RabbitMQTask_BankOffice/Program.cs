using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQTask_BankOffice
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_tour", type: "topic");
                channel.ExchangeDeclare(exchange: "dlx_exchange", type:"direct");

                var queueArgs = new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange", "dlx_exchange" },
                    {"x-dead-letter-routing-key", "dlx_key" }
                };

                channel.QueueDeclare("BankOffice_Queue", true, false, false ,arguments: queueArgs);
                List<string> bindings = new List<string>();
                bindings.Add("tour.booked");
                bindings.Add("tour.cancelled");

                foreach (var bindingKey in bindings)
                {
                    channel.QueueBind(queue: "BankOffice_Queue",
                                      exchange: "topic_tour",
                                      routingKey: bindingKey,
                                      arguments: queueArgs);
                }

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                                      routingKey,
                                      message);
                    //channel.BasicNack(ea.DeliveryTag, false, false);
                };
                channel.BasicConsume(queue: "BankOffice_Queue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
