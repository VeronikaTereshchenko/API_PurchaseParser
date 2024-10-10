using System.Diagnostics;
using System.Net;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Parser._ASP.Net.Interfaces;
using Serilog;
using Serilog.Sinks;

namespace Parser._ASP.Net.Controllers.Parsers
{
    public class HtmlLoader : IPageLoader
    {
        private HttpClient _httpClient;
        private ILogger<HtmlLoader> _logger;

        public HtmlLoader(IHttpClientFactory httpClientFactory, ILogger<HtmlLoader> logger)
        {
            _httpClient = httpClientFactory.CreateClient();

            _logger = logger;

            //without that header doesn't work
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public async Task<string> GetPageAsync(string currentUrl)
        {
            var response = await _httpClient.GetAsync(currentUrl);

            //the error about not accessing the page is caught in the PurchaseController.cs
            if (response is {StatusCode: HttpStatusCode.OK }) 
            {
                return await response.Content.ReadAsStringAsync();
            }

            //logging
            var errorInfo = "Link couldn't be accessed: {0}. StatCode {1}".Replace("{0}", currentUrl).Replace("{1}", response.StatusCode.ToString());
            _logger.LogDebug(errorInfo);
            Console.WriteLine(errorInfo);
            
            return string.Empty;
        }
    }
}
