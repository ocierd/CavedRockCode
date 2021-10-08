

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CavedRockCode.App.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CavedRockCode.App.Integrations
{
    public class CavedRockApiClient : ICavedRockApiClient
    {
        private readonly HttpClient HttpClient;
        private readonly ILogger<CavedRockApiClient> Logger;
        public CavedRockApiClient(HttpClient client, IConfiguration config, ILogger<CavedRockApiClient> logger)
        {
            Logger = logger;
            HttpClient = client;
            HttpClient.BaseAddress = new System.Uri(config.GetValue<string>("CarvedRockApiUrl"));
        }

        public async Task<IEnumerable<Product>> GetProducts(string category = null)
        {
            try
            {

                string requestUri = "products";
                if (!string.IsNullOrWhiteSpace(category))
                {
                    requestUri += $"?category={category}";
                }
                Logger.LogInformation("Se solicitará información de los productos", HttpClient);
                IEnumerable<Product> products = await HttpClient.GetFromJsonAsync<IEnumerable<Product>>(requestUri);
                return products;
            }
            catch (System.Exception exception)
            {
                Logger.LogError(exception, "Error al intentar obtener los productos");
                throw;
            }

        }
    }

}