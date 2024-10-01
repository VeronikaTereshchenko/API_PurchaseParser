using System.Net;
using System.Web;
using Parser._ASP.Net.Interfaces;
using Parser._ASP.Net.Models.Purchases;

namespace Parser._ASP.Net.Controllers.Parsers
{
    public class HtmlLoader
    {
        private HttpClient _httpClient; 

        public HtmlLoader(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            //without that header doesn't work
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public async Task<string> GetPageAsync(int num, string phrase, string url)
        {
            //кодируем слово, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var encodeName = HttpUtility.UrlEncode(phrase);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = url.Replace("{PHRASE}", encodeName).Replace("{NUMBER}", num.ToString());

            var response = await _httpClient.GetAsync(currentUrl);

            //the error about not accessing the page is caught in the PurchaseController.cs
            if(response != null && response.StatusCode == HttpStatusCode.OK) 
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine($"link couuldn't be accessed: {url}");
            return string.Empty;
        }
    }
}
