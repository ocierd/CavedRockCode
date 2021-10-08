using System.Collections.Generic;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Interfaces
{
    public interface IProductLogic
    {
        IEnumerable<Product> GetProductosForCategory(string category);
    }
}
