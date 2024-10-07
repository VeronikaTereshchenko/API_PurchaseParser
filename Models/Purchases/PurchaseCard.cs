using Parser._ASP.Net.Interfaces;

namespace Parser._ASP.Net.Models.Purchases
{
    public class PurchaseCard
    {
        public string Law { get; set; }
        public string Number { get; set; }
        public string PurchaseObject { get; set; }
        public string Organization { get; set; }
        public decimal StartPrice { get; set; }
    }
}
