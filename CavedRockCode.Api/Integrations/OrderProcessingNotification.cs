using System;
using RabbitMQ.Client;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;
using Microsoft.Extensions.Configuration;
using CavedRockCode.Api.ApiModels;
using System.Text.Json;
using Serilog;

namespace CavedRockCode.Api.Integrations
{
    public class OrderProcessingNotification : IOrderProcessingNotification, IDisposable
    {
        private readonly IConnection Connection;
        private readonly IModel Channel;

        private const string QueueName = "quickorder.received";

        public OrderProcessingNotification(IConfiguration configuration)
        {
            var connFactory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMqHost")
            };
            Connection = connFactory.CreateConnection();
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void QuickOrderReceived(QuickOrder order, int customerId, Guid orderId)
        {
            var orderReceviedMessage = new QuickOrderReceivedMessage { Order = order, CustomerId = customerId, OrderId = orderId };
            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(orderReceviedMessage);
            Channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: messageBytes);

            Log.ForContext(propertyName: "Body", orderReceviedMessage, destructureObjects: true)
            .Information(messageTemplate: "Published quick order notification");

        }


        public void Dispose()
        {
            Channel?.Dispose();
            Connection?.Dispose();
        }

    }

}