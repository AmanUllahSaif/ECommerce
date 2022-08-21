using Ecommerce.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.DAL;

namespace Ecommerce.Repo.Handler
{
   public class DiscountBannerHandler: IGenericRepository<SaleBanner>
    {
        private EcommerceDBEntities _context;

        public DiscountBannerHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<SaleBanner> List
        {
            get
            {
                return _context.SaleBanners.Where(x=>x.IsActive==true);
            }
        }
        public void Add(SaleBanner model)
        {
            _context.SaleBanners.Add(model);
            _context.SaveChanges();
        }
        public void Update(SaleBanner model)
        {
            _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(SaleBanner entity)
        {
            entity.IsActive = false;
            this.Update(entity);
        }
        public SaleBanner FindById(long Id)
        {
            return _context.SaleBanners.Find(Id);
        }
    }
}
