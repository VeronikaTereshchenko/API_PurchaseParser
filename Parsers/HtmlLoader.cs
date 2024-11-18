using System.Diagnostics;
using System.Net;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using PurchaseSiteParser.Interfaces;
using Serilog;
using Serilog.Sinks;

namespace PurchaseSiteParser.Controllers.Parsers
{
    public class HtmlLoader : IPageLoader
    {
        private HttpClient _httpClient;
        private Serilog.ILogger _logger;

        public HtmlLoader(IHttpClientFactory httpClientFactory, Serilog.ILogger logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            _logger = logger;
        }

        public async Task<string> GetPageAsync(string currentUrl)
        {
            var response = await _httpClient.GetAsync(currentUrl);

            //the error about not accessing the page is caught in the PurchaseController.cs
            if (response is {StatusCode: HttpStatusCode.OK }) 
            {
                return await response.Content.ReadAsStringAsync();
            }

            _logger.Warning($"Link couldn't be accessed: {currentUrl}. StatCode {response.StatusCode.ToString()}");
            
            return string.Empty;
        }
    }
}
