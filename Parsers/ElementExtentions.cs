using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace Parser._ASP.Net.Parsers
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
            var resultString = string.Join(string.Empty, Regex.Matches(valuseStr, @"\d+\,?").OfType<Match>().Select(m => m.Value));

            if (decimal.TryParse(resultString, out var decimalNum))
                return decimalNum;

            return 0;
        }
    }
}
