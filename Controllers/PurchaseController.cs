using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Models.Purchases;
using Microsoft.Extensions.Options;
using Parser._ASP.Net.Interfaces;
using Parser._ASP.Net.Parsers.Purchases;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private ICustomParser _parser;

        public PurchaseController(ICustomParser parserWorker) 
        {
            _parser = parserWorker;
        }

        [Route("api/zakupki/purchases")]
        [HttpPost]
        public async Task<IActionResult> ParsePurchases() 
        {
            var parsedPurchasesList = await _parser.GetProductsAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
