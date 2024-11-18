using Microsoft.Extensions.Caching.Memory;

namespace PurchaseSiteParser.Interfaces
{
    public interface IPageLoader
    {
        public Task<string> GetPageAsync(string url);
    }
}
