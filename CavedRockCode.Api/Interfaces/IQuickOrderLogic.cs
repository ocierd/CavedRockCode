using System;
using CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Interfaces
{
    public interface IQuickOrderLogic
    {

        Guid PlaceQuickOrder(QuickOrder quickOrder, int customerId);
    }
}
