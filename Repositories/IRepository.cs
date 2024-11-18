using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseSiteParser.Repositories
{
    public interface IRepository<T>  where T : class
    {
        IEnumerable<T> GetItemsList();
        T GetItem(int id);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
