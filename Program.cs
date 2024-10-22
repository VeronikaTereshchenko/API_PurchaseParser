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

        builder.Services.AddSerilog(lc => lc
            .ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddScoped<IWebParser, PurchaseParser>();
        builder.Services.AddScoped<IPageLoader, HtmlLoader>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}