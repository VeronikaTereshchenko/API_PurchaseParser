using Microsoft.Extensions.Caching.Memory;

namespace Parser._ASP.Net.Interfaces
{
    public interface IPageLoader
    {
        public Task<string> GetPageAsync(string url);
    }
}
