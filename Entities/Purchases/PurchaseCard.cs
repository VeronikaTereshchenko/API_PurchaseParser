using System.Text.Json.Serialization;

namespace PurchaseSiteParser.Entities.Purchases
{
    public class PurchaseCard
    {
        public int Id { get; set; }
        public string Law { get; set; }
        public string Number { get; set; }
        public string PurchaseObject { get; set; }
        public string Organization { get; set; }
        public decimal StartPrice { get; set; }


        public int PurchaseParsingResultId { get; set; }
        [JsonIgnore]
        public PurchaseParsingResult PurchaseParsingResult { get; set; }
    }
}
