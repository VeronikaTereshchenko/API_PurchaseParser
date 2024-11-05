using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser._ASP.Net.Data.DataContext;
using Parser._ASP.Net.Data.Entities.Purchases;
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
            var parsedPurchasesList = await _parser.GetPagesInfoAsync();

            return Ok(parsedPurchasesList);
        }
    }
}
