namespace Parser._ASP.Net.Interfaces
{
    public interface IPageLoader
    {
        public Task<string> GetPageAsync(int num, string phrase, string url);
    }
}
