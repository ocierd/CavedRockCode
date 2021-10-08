using System;
using System.Collections.Generic;
using System.Linq;
using CavedRockCode.Api.Interfaces;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;
using Microsoft.Extensions.Logging;

namespace CavedRockCode.Api.Domain
{
    public class ProductLogic : IProductLogic
    {
        private readonly ILogger<ProductLogic> Logger;
        private readonly List<string> ValidCategories = new List<string> { "all", "boots", "climbing gear", "kayaks" };

        public ProductLogic(ILogger<ProductLogic> logger)
        {
            Logger = logger;
        }

        public IEnumerable<Product> GetProductosForCategory(string category)
        {
            Logger.LogInformation("Starting logic to get products for category");

            if (ValidCategories.All(validCategory => !string.Equals(validCategory, category, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ApplicationException($"Unrecognized category: {category}. Valid categories are: [{string.Join(",", ValidCategories)}]");
            }
            if (string.Equals(category, "kayaks", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Not implemented yet! No kayaks have been defined in 'database' yet!!!!!");
            }

            return GetAllProducts().Where(product =>
            string.Equals(category, "all", StringComparison.InvariantCultureIgnoreCase) ||
            string.Equals(product.Category, category, StringComparison.InvariantCultureIgnoreCase));
        }


        private static IEnumerable<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product{Id=1, Name="Trailblazer", Category="boots", Price=69.99, Description="Great support in this high-uoto take you to great heights and trails." },
                new Product{Id=2, Name="Coastliner", Category="boots",Price=49.99,  Description="Easy in and out with this lightweight" },
                new Product{Id=3, Name="Woodsman", Category="boots",Price=64.99, Description="All the insulation and support you need" },
                new Product{Id=4, Name="Billy", Category="boots",Price=79.99, Description="Get up and down rocky" },
            };
        }
    }
}