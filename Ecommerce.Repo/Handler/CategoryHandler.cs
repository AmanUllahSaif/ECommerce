using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL;
using System.Data.Entity;
using Ecommerce.DAL.Enums;

namespace Ecommerce.BLL.Handler
{
    public class CategoryHandler : IGenericRepository<Category>
    {
        public CategoryHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        private EcommerceDBEntities _context;
        public IEnumerable<Category> List
        {
            get
            {
                return _context.Categories.Where(x => x.IsActive == true);
            }
        }
       public void Add(Category entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();
        }
       public void Delete(Category entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
       public void Update(Category entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
       public Category FindById(long Id)
        {
            return _context.Categories.Find(Id);
        }

        public IEnumerable<Category> NavigationHeadings()
        {
            return this.List.Where(x => x.Type == (int)CategoryType.NavigationHeading && x.IsActive == true);
        }
        public IEnumerable<Category> NavigationCategory()
        {
            return this.List.Where(x => x.Type == (int)CategoryType.NavigationCategory && x.IsActive == true);
        }

        public IEnumerable<Category> NavigationLinks()
        {
            return this.List.Where(x => x.Type == (int)CategoryType.NavigationLink && x.IsActive == true);
        }
    }
}
