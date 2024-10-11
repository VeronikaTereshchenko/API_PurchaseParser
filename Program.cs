using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Parsers.Purchases;
using Parser._ASP.Net.Interfaces;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using System.Globalization;
using Polly;
using Parser._ASP.Net.Parsers;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.Configure<PurchaseSettings>(
            builder.Configuration.GetSection(PurchaseSettings.PurchaseSection) ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));

        builder.Services.Configure<MyRateLimitOptions>(
            builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit) ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));

        var myRateOptions = new MyRateLimitOptions();
        builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myRateOptions);

        builder.Services.AddSerilog((ls) => ls
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}", "Log.txt")));

        builder.Services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "purchaseLimiter", options =>
            {
                options.PermitLimit = myRateOptions.PermitLimit;
                options.Window = TimeSpan.FromSeconds(myRateOptions.Window);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = myRateOptions.QueueLimit;
            })
            .AddConcurrencyLimiter(policyName: "concurrencyLimiter", options =>
            {
                options.PermitLimit = myRateOptions.PermitLimit;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = myRateOptions.QueueLimit;
            })
            .OnRejected = (context, CancellationToken) => 
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                var controllerName = context.HttpContext.Request.RouteValues["controller"];
                var actionName = context.HttpContext.Request.RouteValues["action"];
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                    .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                    .LogWarning("OnRejected: {GetUserEndPoint}/{ActionName}", controllerName, actionName);

                return new ValueTask();
            });
        
        builder.Services.AddLogging(build => build.AddConsole());

        builder.Services.AddScoped<IWebParser, PurchaseParser>();
        builder.Services.AddScoped<IPageLoader, HtmlLoader>();

        builder.Services.AddHttpClient("PoliceClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(60);
        })
            .AddPolicyHandler(PolicyRegistry.GetRateLimitPolicy());


        builder.Services.AddMemoryCache();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}