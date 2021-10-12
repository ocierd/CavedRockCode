using System;
using CavedRockCode.Api.Interfaces;
using Microsoft.Extensions.Logging;
using CavedRockCode.Api.ApiModels;
using CavedRockCode.Api.Integrations;
using CavedRockCode.Api.Repository;
using System.Threading.Tasks;

namespace CavedRockCode.Api.Domain
{
    public class QuickOrderLogic : IQuickOrderLogic
    {
        private readonly ILogger<QuickOrderLogic> Logger;

        private readonly IOrderProcessingNotification OrderProcessingNotification;

        private readonly ICavedRepository CavedRepository;

        public QuickOrderLogic(ILogger<QuickOrderLogic> logger, IOrderProcessingNotification orderProcessingNotification
        , ICavedRepository cavedRepository)
        {
            Logger = logger;
            OrderProcessingNotification = orderProcessingNotification;
            CavedRepository = cavedRepository;
        }
        public async Task<Guid> PlaceQuickOrder(QuickOrder quickOrder, int customerId)
        {
            Logger.LogInformation("Placing order and send update for inventory...");
            Guid orderId = Guid.NewGuid();
            //Persist order to database or wherever
            await CavedRepository.SubmitProduct(quickOrder, customerId, orderId);

            //post "orderplaced" event to rabbitmg
            OrderProcessingNotification.QuickOrderReceived(quickOrder, customerId, orderId);
            return orderId;
        }
    }
}
