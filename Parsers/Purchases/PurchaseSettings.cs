using PurchaseSiteParser.Interfaces;

namespace PurchaseSiteParser.Purchases
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
