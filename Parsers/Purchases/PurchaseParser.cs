using AngleSharp.Html.Dom;
using PurchaseSiteParser.Interfaces;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Caching.Memory;
using AngleSharp.Dom;
using System;
using System.Web;
using Serilog.Data;
using PurchaseSiteParser.Entities.Purchases;
using PurchaseSiteParser.DataContext;
using Microsoft.EntityFrameworkCore;
using PurchaseSiteParser.Repositories;
using PurchaseSiteParser.Parsers;

namespace PurchaseSiteParser.Purchases
{
    public class PurchaseParser : ISiteParser
    {
        private readonly IPageLoader _htmlLoader;
        private readonly PurchaseSettings _purchaseSettings;
        private readonly IRepository<PurchaseParsingResult> _db;

        public PurchaseParser(IOptions<PurchaseSettings> purchaseOption, IPageLoader htmlLoader, PurchaseContext purchaseContext)
        {
            _purchaseSettings = purchaseOption.Value;
            _htmlLoader = htmlLoader;
            _db = new PurchaseRepository(purchaseContext);
        }

        public async Task<PurchaseParsingResult> GetPagesInfoAsync()
        {
            var parsedInfo = new List<PurchaseCard>();

            for (int pageNum = _purchaseSettings.FirstPageNum; pageNum <= _purchaseSettings.LastPageNum; pageNum++)
            {
                var currentUrl = GetUrl(pageNum);

                var source = await _htmlLoader.GetPageAsync(currentUrl);

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

            var foundPurchases = new PurchaseParsingResult()
            {
                PurchaseName = _purchaseSettings.PurchaseName,
                PagesPeriod = $"search through pages {_purchaseSettings.FirstPageNum} to {_purchaseSettings.LastPageNum}",
                PurchasesListCount = parsedInfo.Count,
                PurchasesCardsList = parsedInfo
            };

            _db.Add(foundPurchases);
            _db.Save();

            return foundPurchases;
        }

        private List<PurchaseCard> Parse(IHtmlDocument document)
        {
            //ищем карточки (карточка хранит инф. об одном объекте, имя объекта задаётся в app_PurchaseSettings.json)
            //search for a card
            var purchaseCardsHtml = document.QuerySelectorAll("div.row.no-gutters.registry-entry__form.mr-0");

            var cards = new List<PurchaseCard>();

            if (purchaseCardsHtml == null)
            {
                return cards;
            }

            foreach (var purchaseCardHtml in purchaseCardsHtml)
            {
                var card = new PurchaseCard()
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

        private string GetUrl(int pageNum)
        {
            var encodeName = HttpUtility.UrlEncode(_purchaseSettings.PurchaseName);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 

            return _purchaseSettings.BaseUrl.Replace("{PHRASE}", encodeName).Replace("{NUMBER}", pageNum.ToString());
        }
    }

}

