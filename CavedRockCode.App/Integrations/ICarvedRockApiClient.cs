using System.Collections.Generic;
using System.Threading.Tasks;
using CavedRockCode.App.Models;

namespace CavedRockCode.App.Integrations
{
    public interface ICavedRockApiClient
    {
        Task<IEnumerable<Product>> GetProducts(string category = null);
    }

}