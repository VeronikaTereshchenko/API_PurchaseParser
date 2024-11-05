using AngleSharp.Html.Dom;
using Microsoft.Extensions.Caching.Memory;
using Parser._ASP.Net.Data.Entities.Purchases;

namespace Parser._ASP.Net.Interfaces
{
    public interface IWebParser
    {
        Task<PurchaseParsingResult> GetPagesInfoAsync();
    }
}
