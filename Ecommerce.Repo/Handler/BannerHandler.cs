using Ecommerce.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL;

namespace Ecommerce.Repo.Handler
{
  public class BannerHandler: IGenericRepository<Banner>
    {
        private EcommerceDBEntities _context;
        public BannerHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<Banner> List
        {
            get
            {
                return _context.Banners.Where(x => x.IsActive == true).OrderByDescending(x=>x.CreatedOn);
            }
        }
        public void Add(Banner model)
        {
            _context.Banners.Add(model);
            _context.SaveChanges();
        }
        public void Update(Banner model)
        {
            _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(Banner entity)
        {
            entity.IsActive = false;
            this.Update(entity);
        }
        public Banner FindById(long Id)
        {
            return _context.Banners.Find(Id);
        }

    }
}
