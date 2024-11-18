using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace PurchaseSiteParser.Parsers
{
    public static class IElementExtentions
    {
        public static string GetTextContent(this IElement element, string selector)
        {
            if (element == null)
                return " information is not found";

            var textHtml = element.QuerySelector(selector);

            if (textHtml == null)
                return " information is not found";

            var textContent = textHtml.TextContent;
            textContent = Regex.Replace(textContent, @"[\r\n\t]", " ");
            textContent = Regex.Replace(textContent, @"\s+", " ");

            return textContent;
        }

        public static decimal GetDecimalNum(this IElement element, string valuseStr)
        {
            var numHtml = element.QuerySelector(valuseStr);

            var numStr = numHtml.TextContent;

            var resultString = string.Join(string.Empty, Regex.Matches(numStr, @"\d+\,?").OfType<Match>().Select(m => m.Value));

            if (decimal.TryParse(resultString, out var decimalNum))
                return decimalNum;

            return 0;
        }
    }
}
