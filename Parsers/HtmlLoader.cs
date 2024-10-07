using System.Net;
using System.Web;
using Parser._ASP.Net.Interfaces;
using Serilog;
using Serilog.Sinks;

namespace Parser._ASP.Net.Controllers.Parsers
{
    public class HtmlLoader : IPageLoader
    {
        private HttpClient _httpClient; 
        private Serilog.Core.Logger _logger;

        public HtmlLoader(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            _logger.ForContext<HtmlLoader>();

            //without that header doesn't work
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public async Task<string> GetPageAsync(int num, string phrase, string url)
        {
            var encodeName = HttpUtility.UrlEncode(phrase);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = url.Replace("{PHRASE}", encodeName).Replace("{NUMBER}", num.ToString());

            var response = await _httpClient.GetAsync(currentUrl);

            //the error about not accessing the page is caught in the PurchaseController.cs
            if(response is {StatusCode: HttpStatusCode.OK }) 
            {
                return await response.Content.ReadAsStringAsync();
            }

            _logger.Error("link couldn't be accessed: {0}. StatCode {1}", currentUrl, response.StatusCode.ToString());
            return string.Empty;
        }
    }
}
