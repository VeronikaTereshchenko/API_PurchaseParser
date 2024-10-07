using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.Interfaces
{
    public interface IWebParser
    {
        Task<PurchaseParsingResult> GetPageInfoAsync();
    }
}
