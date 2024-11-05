using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Interfaces;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private IWebParser _parser;

        public PurchasesController(IWebParser parser) 
        {
            _parser = parser;
        }

        [Route("api/zakupki/purchases")]
        [HttpGet]
        public async Task<IActionResult> GetPurchases() 
        {
            var parsedPurchasesList = await _parser.GetPageInfoAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
