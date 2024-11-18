using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseSiteParser.DataContext;
using PurchaseSiteParser.Entities.Purchases;
using PurchaseSiteParser.Interfaces;

namespace PurchaseSiteParser.Controllers
{
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private ISiteParser _parser;

        public PurchasesController(ISiteParser parser) 
        {
            _parser = parser;
        }

        [Route("api/zakupki/purchases")]
        [HttpGet]
        public async Task<IActionResult> GetPurchases() 
        {
            var parsedPurchasesList = await _parser.GetPagesInfoAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
