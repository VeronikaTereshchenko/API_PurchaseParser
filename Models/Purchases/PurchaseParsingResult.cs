namespace Parser._ASP.Net.Models.Purchases
{
    public class PurchaseParsingResult
    {
        public string PurchaseName { get; set; } = string.Empty;

        public string PagesPeriod { get; set; } = string.Empty;

        public int PurchasesListCount { get; set; } = 0;

        public List<PurchaseCard> PurchasesList { get; set; } = new List<PurchaseCard>();
    }
}
