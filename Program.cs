using PurchaseSiteParser.Controllers.Parsers;
using PurchaseSiteParser.Purchases;
using PurchaseSiteParser.Interfaces;
using Serilog;
using System.Globalization;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using PurchaseSiteParser.DataContext;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        builder.Services.AddDbContext<PurchaseContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
        });

        builder.Services.Configure<PurchaseSettings>(
            builder.Configuration.GetSection(PurchaseSettings.PurchaseSection) ??
            throw new InvalidOperationException("Connection string 'PurchaseSettings' not found."));

        builder.Services.AddSerilog(ls => ls
            .ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddScoped<ISiteParser, PurchaseParser>();
        builder.Services.AddScoped<IPageLoader, HtmlLoader>();

        builder.Services.AddHttpClient();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}