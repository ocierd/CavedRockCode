using System;
using CavedRockCode.Api.Interfaces;
using Microsoft.Extensions.Logging;
using CavedRockCode.Api.ApiModels;
using CavedRockCode.Api.Integrations;

namespace CavedRockCode.Api.Domain
{
    public class QuickOrderLogic : IQuickOrderLogic
    {
        private readonly ILogger<QuickOrderLogic> Logger;

        private readonly IOrderProcessingNotification OrderProcessingNotification;

        public QuickOrderLogic(ILogger<QuickOrderLogic> logger, IOrderProcessingNotification orderProcessingNotification)
        {
            Logger = logger;
            OrderProcessingNotification = orderProcessingNotification;
        }
        public Guid PlaceQuickOrder(QuickOrder quickOrder, int customerId)
        {
            Logger.LogInformation("Placing order and send update for inventory...");
            var orderId = Guid.NewGuid();
            //Persist order to database or wherever


            //post "orderplaced" event to rabbitmg
            OrderProcessingNotification.QuickOrderReceived(quickOrder, customerId, orderId);
            return orderId;
        }
    }
}
