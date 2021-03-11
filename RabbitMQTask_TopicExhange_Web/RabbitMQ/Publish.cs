using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTask_TopicExhange_Web.RabbitMQ
{
    public static class Publish
    {
        public static void PublishMessage(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                var props = channel.CreateBasicProperties();

                channel.ExchangeDeclare(exchange: "topic_tour",
                                        type: "topic");

                var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                var message = (args.Length > 1)
                              ? string.Join(" ", args.Skip(1).ToArray())
                              : "No message";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic_tour",         
                                     routingKey: routingKey,
                                     basicProperties: props,
                                     body: body);
            }
        }
    }
}
