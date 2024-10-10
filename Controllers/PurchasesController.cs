using Microsoft.AspNetCore.Mvc;
using Parser._ASP.Net.Interfaces;
using Microsoft.AspNetCore.RateLimiting;

namespace Parser._ASP.Net.Controllers
{
    [ApiController]
    [EnableRateLimiting("purchaseLimiter")]
    public class PurchasesController : ControllerBase
    {
        private IWebParser _parser;

        public PurchasesController(IWebParser parser) 
        {
            _parser = parser;
        }

        [Route("api/[controller]/[action]")]
        [EnableRateLimiting("concurrencyLimiter")]
        [HttpGet]
        public async Task<IActionResult> GetPurchases() 
        {
            var parsedPurchasesList = await _parser.GetPageInfoAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
