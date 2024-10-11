using Polly;
using Polly.Retry;
using System.Net;

namespace Parser._ASP.Net.Parsers
{
    public static class PolicyRegistry
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRateLimitPolicy()
        {
            return Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(5),
                    onRetry: (response, retryCount, context) =>
                    {
                        Console.WriteLine($"Rate limit exceeded. Waiting for 30 seconds before retrying...");
                    });
        }
    }
}
