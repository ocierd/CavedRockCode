using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CavedRockCode.Api.ApiModels;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Repository
{
    public interface ICavedRepository
    {

        Task<List<Product>> GetProducts(string category);

        Task SubmitProduct(QuickOrder order,int customerId,Guid orderId);

    }

}