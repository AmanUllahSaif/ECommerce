using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Handler
{
    public class VendorHandler : IGenericRepository<Vendor>
    {
        private EcommerceDBEntities _context;
        public VendorHandler(EcommerceDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<Vendor> List
        {
            get
            {
                return _context.Vendors.Where(x => x.IsActive == true);
            }
        }

        public void Add(Vendor model)
        {
            _context.Vendors.Add(model);
            _context.SaveChanges();
        }

        public void Update(Vendor model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Vendor entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
        public Vendor FindById(long Id)
        {
            return _context.Vendors.Find(Id);
        }
    }
}
