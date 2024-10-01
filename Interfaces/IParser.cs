using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.Interfaces
{
    public interface IParser
    {
        public List<Card> Parse(IHtmlDocument document);
    }
}
