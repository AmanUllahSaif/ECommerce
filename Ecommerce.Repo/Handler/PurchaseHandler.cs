using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Handler
{
    public class PurchaseHandler : IGenericRepository<Purchase>
    {

        private EcommerceDBEntities _context;

        public PurchaseHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<Purchase> List
        {
            get
            {
              return  _context.Purchases.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn);
            }
        }

        public void Add(Purchase entity)
        {
            _context.Purchases.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Purchase entity)
        {
            entity.IsActive = false;
            Update(entity);
        }

        public Purchase FindById(long Id)
        {
          return  _context.Purchases.Find(Id);
        }

        public void Update(Purchase entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
