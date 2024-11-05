using Parser._ASP.Net.Controllers.Parsers;
using Parser._ASP.Net.Parsers.Purchases;
using Parser._ASP.Net.Interfaces;
using Serilog;
using System.Globalization;
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
            throw new InvalidOperationException("Connection string 'PurchaseSettings' not found."));

        builder.Services.AddSerilog((ls) => ls
            .ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddScoped<IWebParser, PurchaseParser>();
        builder.Services.AddScoped<IPageLoader, HtmlLoader>();

        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}