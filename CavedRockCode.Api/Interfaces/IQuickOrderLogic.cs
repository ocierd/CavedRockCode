using System;
using System.Threading.Tasks;
using CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Interfaces
{
    public interface IQuickOrderLogic
    {

        Task<Guid> PlaceQuickOrder(QuickOrder quickOrder, int customerId);
    }
}
