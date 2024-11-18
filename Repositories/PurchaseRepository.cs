using Microsoft.EntityFrameworkCore;
using PurchaseSiteParser.DataContext;
using PurchaseSiteParser.Entities.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseSiteParser.Repositories
{
    public class PurchaseRepository : IRepository<PurchaseParsingResult>
    {
        private PurchaseContext _db;

        public PurchaseRepository(PurchaseContext purchaseContext) 
        {
            _db = purchaseContext;
        }

        public void Add(PurchaseParsingResult item)
        {
            _db.PurchaseParsingResults.Add(item);
        }

        public void Delete(int id)
        {
            var item = _db.PurchaseParsingResults.Find(id);

            if(item != null)
                _db.PurchaseParsingResults.Remove(item);
        }

        public PurchaseParsingResult GetItem(int id)
        {
            return _db.PurchaseParsingResults
                .Include(u => u.PurchasesCardsList)
                .FirstOrDefault(u => u.id == id)!;
        }

        public IEnumerable<PurchaseParsingResult> GetItemsList()
        {
            return _db.PurchaseParsingResults.Include(u => u.PurchasesCardsList).ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(PurchaseParsingResult item)
        {
            _db.PurchaseParsingResults.Update(item);
        }
    }
}
