namespace PurchaseSiteParser.Entities.Purchases
{
    public class PurchaseParsingResult
    {
        public int id { get; set; }
        public string PurchaseName { get; set; }

        public string PagesPeriod { get; set; }

        public int PurchasesListCount { get; set; }


        public ICollection<PurchaseCard> PurchasesCardsList { get; set; } = new List<PurchaseCard>();
    }
}
