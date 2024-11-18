using AngleSharp.Html.Dom;
using Microsoft.Extensions.Caching.Memory;
using PurchaseSiteParser.Entities.Purchases;

namespace PurchaseSiteParser.Interfaces
{
    public interface ISiteParser
    {
        Task<PurchaseParsingResult> GetPagesInfoAsync();
    }
}
