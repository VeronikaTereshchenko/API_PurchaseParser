using AngleSharp.Html.Dom;
using Parser._ASP.Net.Models.Purchases;
using Parser._ASP.Net.Interfaces;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Parser;

namespace Parser._ASP.Net.Parsers.Purchases
{
    public class PurchaseParser : ICustomParser
    {
        private ILoader _htmlLoader;
        private PurchaseSettings _purchaseSettings;

        public PurchaseParser(IOptions<PurchaseSettings> purchaseOption, ILoader htmlLoader)
        {
            _purchaseSettings = purchaseOption.Value;
            _htmlLoader = htmlLoader;
        }

        public async Task<FoundPurchases> GetProductsAsync()
        {
            var parsedInfo = new List<Card>();

            for (int pageNum = _purchaseSettings.FirstPageNum; pageNum <= _purchaseSettings.LastPageNum; pageNum++)
            {
                var source = await _htmlLoader.GetPageAsync(pageNum, _purchaseSettings.PurchaseName, _purchaseSettings.BaseUrl);

                if (string.IsNullOrEmpty(source))
                    continue;

                var htmlParser = new HtmlParser();
                var document = await htmlParser.ParseDocumentAsync(source);

                if (document == null)
                    continue;

                //достаём инф. из каждой карточки на странице по тегам и классам
                //retrieve information from each card on the page by tags and classes
                var result = Parse(document);

                parsedInfo.AddRange(result);
            }

            var foundPurchases = new FoundPurchases()
            {
                PurchaseName = _purchaseSettings.PurchaseName,
                PagesPeriod = $"search through pages {_purchaseSettings.FirstPageNum} to {_purchaseSettings.LastPageNum}",
                PurchasesListCount = parsedInfo.Count,
                PurchasesList = parsedInfo
            };

            return foundPurchases;
        }

        public List<Card> Parse(IHtmlDocument document)
        {
            //ищем карточки (карточка хранит инф. об одном объекте, имя объекта задаётся в app_PurchaseSettings.json)
            //search for a card
            var purchaseCardsHtml = document.QuerySelectorAll("div.row.no-gutters.registry-entry__form.mr-0");

            var cards = new List<Card>();

            if (purchaseCardsHtml == null)
            {
                return cards;
            }

            foreach (var purchaseCardHtml in purchaseCardsHtml)
            {
                var card = new Card()
                {
                    Law = purchaseCardHtml.GetTextContent( "div.col-9.p-0.registry-entry__header-top__title.text-truncate"),
                    Number = purchaseCardHtml.GetTextContent("div.registry-entry__header-mid__number a"),
                    PurchaseObject = purchaseCardHtml.GetTextContent("div.registry-entry__body-value"),
                    Organization = purchaseCardHtml.GetTextContent("div.registry-entry__body-href"),
                    StartPrice = purchaseCardHtml.GetDecimalNum("div.price-block__value")
                };

                cards.Add(card);
            }

            return cards;
        }
    }

}

