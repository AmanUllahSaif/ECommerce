using Ecommerce.BLL;
using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Ecommerce.BLL.Handler
{
   public class AttributeHandler : IGenericRepository<DAL.Attribute>
    {
        private EcommerceDBEntities _context;
        public AttributeHandler(EcommerceDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<DAL.Attribute> List
        {
            get
            {
                return _context.Attributes.Where(x => x.IsActive == true);
            }
        }

        public void Add(DAL.Attribute model)
        {
            _context.Attributes.Add(model);
            _context.SaveChanges();
        }

        public void Update(DAL.Attribute model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(DAL.Attribute entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
        public DAL.Attribute FindById(long Id)
        {
            return _context.Attributes.Find(Id);
        }

        public IEnumerable<DAL.Attribute> GetByCategoryId(long Id)
        {
            return this.List.Where(x => x.CategoryId == Id);
        }

    }
}
