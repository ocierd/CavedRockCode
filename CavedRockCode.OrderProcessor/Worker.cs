using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CavedRockCode.OrderProcessor.Models;
using CavedRockCode.OrderProcessor.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CavedRockCode.OrderProcessor
{
    public class Worker : BackgroundService
    {
        private readonly IInventoryRepository InventoryRepository;
        private readonly ILogger<Worker> _logger;
        private readonly IConnection Connection;
        private readonly IModel Channel;
        private const string QueueName = "quickorder.received";
        private readonly EventingBasicConsumer Consumer;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IInventoryRepository invetoryRepository)
        {
            InventoryRepository = invetoryRepository;
            _logger = logger;
            string rabbitMqHost = configuration.GetValue<string>("RabbitMqHost");
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMqHost
            };

            Connection = connectionFactory.CreateConnection();
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Consumer = new EventingBasicConsumer(Channel);
            Consumer.Received += OnProcessQuickOrderRecevied;
        }

        private async void OnProcessQuickOrderRecevied(object sender, BasicDeliverEventArgs eventArgs)
        {
            QuickOrderReceivedMessage orderReceivedMessage = JsonSerializer.Deserialize<QuickOrderReceivedMessage>(eventArgs.Body.Span);

            int currentQuantity = await InventoryRepository.GetInventoryForProduct(orderReceivedMessage.Order.ProductId);
            int quantityAfterOrder = currentQuantity - orderReceivedMessage.Order.Quantity;
            await InventoryRepository.UpdateInventoryForProduct(orderReceivedMessage.Order.ProductId, quantityAfterOrder);
            Log.ForContext("Order received", orderReceivedMessage, true)
            .Information("Received message from queue for processing");

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Channel.BasicConsume(queue: QueueName, autoAck: true, consumer: Consumer);
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
