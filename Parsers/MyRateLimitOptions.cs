namespace Parser._ASP.Net.Parsers.Purchases
{
    public class MyRateLimitOptions
    {
        public const string MyRateLimit = "MyRateLimitOptions";
        public int PermitLimit { get; set; }
        public int Window {  get; set; }
        public int QueueLimit { get; set; }
    }
}
