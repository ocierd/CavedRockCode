using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CavedRockCode.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CavedRockCode.Api.ApiModels;

namespace CavedRockCode.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuickOrderController : ControllerBase
    {

        public readonly ILogger<QuickOrderController> Logger;
        private readonly IQuickOrderLogic QuickOrderLogic;


        public QuickOrderController(ILogger<QuickOrderController> logger, IQuickOrderLogic quickOrderLogic)
        {
            Logger = logger;
            QuickOrderLogic = quickOrderLogic;
        }

        [HttpPost]
        public Guid SubmitQuickOrder(QuickOrder quickOrder)
        {
            return QuickOrderLogic.PlaceQuickOrder(quickOrder, 12345);
        }
    }
}
