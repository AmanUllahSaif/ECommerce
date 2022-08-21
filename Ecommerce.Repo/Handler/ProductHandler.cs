using Ecommerce.BLL;
using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Handler
{
    public class ProductHandler : IGenericRepository<Product>
    {
        private EcommerceDBEntities _context;
        public ProductHandler(EcommerceDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<Product> List
        {
            get
            {
                return _context.Products.Where(x => x.IsActive == true);
            }
        }

        public void Add(Product model)
        {
            _context.Products.Add(model);
            _context.SaveChanges();
        }

        public void Update(Product model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Product entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
        public Product FindById(long Id)
        {
            return _context.Products.Find(Id);
        }

        public IEnumerable<Product> GetFeatured()
        {
           return this.List.Where(x => x.Featured);
        }
    }
}
