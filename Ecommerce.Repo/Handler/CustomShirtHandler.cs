using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Handler
{
    public class CustomShirtHandler : IGenericRepository<CustomShirt>
    {
        EcommerceDBEntities _context;
        public CustomShirtHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<CustomShirt> List
        {
            get
            {
                return _context.CustomShirts.Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn);
            }
        }

        public void Add(CustomShirt entity)
        {
            _context.CustomShirts.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(CustomShirt entity)
        {
            entity.IsActive = false;
            Update(entity);
           
        }

        public CustomShirt FindById(long Id)
        {
           return _context.CustomShirts.Find(Id);
        }

        public CustomShirt GetByType(int type)
        {
            return _context.CustomShirts.FirstOrDefault(x => x.IsActive && x.Type == type);
        }
        public void Update(CustomShirt entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
