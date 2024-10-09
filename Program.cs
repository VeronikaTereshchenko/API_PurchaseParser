using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Parsers.Purchases;
using Parser._ASP.Net.Interfaces;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.Configure<PurchaseSettings>(
            builder.Configuration.GetSection(PurchaseSettings.PurchaseSection));

        builder.Services.AddSerilog((ls) => ls
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}", "Log.txt")));
        
        builder.Services.AddLogging(build => build.AddConsole());

        builder.Services.AddScoped<IWebParser, PurchaseParser>();
        builder.Services.AddScoped<IPageLoader, HtmlLoader>();
        
        builder.Services.AddHttpClient();
        
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}