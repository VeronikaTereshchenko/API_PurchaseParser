using Parser._ASP.Net.Interfaces;

namespace Parser._ASP.Net.Parsers.Purchases
{
    public class PurchaseSettings
    {
        public const string PurchaseSection = "PurchaseSettings";

        public string BaseUrl { get; set; }

        public string PurchaseName { get; set; }

        public int FirstPageNum { get; set; }

        public int LastPageNum { get; set; }
    }
}
