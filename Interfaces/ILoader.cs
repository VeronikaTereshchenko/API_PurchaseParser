namespace Parser._ASP.Net.Interfaces
{
    public interface ILoader
    {
        public Task<string> GetPageAsync(int num, string phrase, string url);
    }
}
