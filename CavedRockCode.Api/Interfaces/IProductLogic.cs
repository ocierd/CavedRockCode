using System.Collections.Generic;
using System.Threading.Tasks;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Interfaces
{
    public interface IProductLogic
    {
        Task<IEnumerable<Product>> GetProductosForCategory(string category);
    }
}
