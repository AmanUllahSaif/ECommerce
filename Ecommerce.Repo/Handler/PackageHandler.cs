using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL;

namespace Ecommerce.BLL.Handler
{
    public class PackageHandler : IGenericRepository<Package>
    {
        EcommerceDBEntities _context;
        public PackageHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<Package> List
        {
            get
            {
                return _context.Packages.Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn);
            }
        }

        public void Add(Package entity)
        {
            _context.Packages.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Package entity)
        {
            entity.IsActive = false;
            Update(entity);
        }

        public Package FindById(long Id)
        {
            return _context.Packages.Find(Id);
        }

        public void Update(Package entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
