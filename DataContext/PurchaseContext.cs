using Microsoft.EntityFrameworkCore;
using PurchaseSiteParser.Entities.Purchases;

namespace PurchaseSiteParser.DataContext
{
    public class PurchaseContext : DbContext
    {
        public PurchaseContext(DbContextOptions options) : base(options) { }

        public DbSet<PurchaseCard> PurchaseCards { get; set; }
        public DbSet<PurchaseParsingResult> PurchaseParsingResults { get; set; }
    }
}
