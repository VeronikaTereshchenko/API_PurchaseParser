using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Models.Purchases;
using Microsoft.Extensions.Options;
using Parser._ASP.Net.Interfaces;
using Parser._ASP.Net.Parsers.Purchases;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;
using Microsoft.Extensions.Caching;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private IWebParser _parser;

        public PurchaseController(IWebParser parser) 
        {
            _parser = parser;
        }

        [Route("api/zakupki/purchases")]
        [HttpPost]
        public async Task<IActionResult> ParsePurchases() 
        {
            var parsedPurchasesList = await _parser.GetPageInfoAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
