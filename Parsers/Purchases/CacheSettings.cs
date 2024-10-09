namespace Parser._ASP.Net.Parsers.Purchases
{
    public class MemoryChacheOptions
    {
        public const string CacheOptions = "MemoryChacheOptions";
        public int CacheMemoryLimitMegabytes {  get; set; }
        public int PhysicalMemoryLimitPercentage { get; set; }

    }
}
