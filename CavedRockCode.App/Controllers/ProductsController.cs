using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CavedRockCode.App.Models;
using CavedRockCode.App.Integrations;

namespace CavedRockCode.App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICavedRockApiClient CavedRockApiClient;

        public ProductsController(ILogger<HomeController> logger, ICavedRockApiClient client)
        {
            _logger = logger;
            CavedRockApiClient = client;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await CavedRockApiClient.GetProducts();
            return View(productos);
        }

    }
}
