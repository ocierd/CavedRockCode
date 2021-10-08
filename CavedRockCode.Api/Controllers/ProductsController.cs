using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CavedRockCode.Api.Interfaces;
using CavedRockCode.Api.ApiModels.CavedRockCode.Api.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CavedRockCode.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLogic _productLogic;


        public ProductsController(IProductLogic productLogic)
        {

            _productLogic = productLogic;

        }

        [HttpGet]
        public IEnumerable<Product> GetProducts(string category = "all")
        {
            //Log.Information("Start controller action GetProducts for category", category);
            Log.ForContext("Category", category)
            .Information("Starting controller action GetProducts");
            
            return _productLogic.GetProductosForCategory(category);
        }
    }
}
