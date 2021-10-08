using System;
using CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Integrations
{
    public interface IOrderProcessingNotification
    {
        void QuickOrderReceived(QuickOrder order, int customerId, Guid orderId);

    }

}