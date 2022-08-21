using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Handler
{
    public class StockHandler : IGenericRepository<Stock>
    {
        public StockHandler(EcommerceDBEntities context)
        {
            _context = context;
        }
        private EcommerceDBEntities _context;
        public IEnumerable<Stock> List
        {
            get
            {
                return _context.Stocks.Where(x => x.IsActive == true);
            }
        }
        public void Add(Stock entity)
        {
            _context.Stocks.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Stock entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
        public void Update(Stock entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public Stock FindById(long Id)
        {
            return _context.Stocks.Find(Id);
        }

        public Stock Find(long Id, List<AttribCombinationDetail> comb)
        {
            var attCombDetail = _context.AttribCombinationDetails.Where(x => x.IsActive);
            List<AttribCombinationDetail> lst = new List<AttribCombinationDetail>();
            List<AttributeCombination> list = new List<AttributeCombination>();
            
            if (comb != null)
            {
                foreach (var item in comb)
                {
                    lst.AddRange(attCombDetail.Where(x => x.AttributeId == item.AttributeId && x.ValueId == item.ValueId));
                }
               var grp = (from com in lst
                           where com.AttributeCombination.ProductId == Id && com.AttributeCombination.IsActive
                           select new
                           {
                               Id = com.CombinationId,
                               ProductId = com.AttributeCombination.ProductId,
                               Stocks = com.AttributeCombination.Stocks.Where(x => x.IsActive).ToList()
                           }).FirstOrDefault();
                if (grp == null)
                {
                    return null;
                }
                return grp.Stocks.FirstOrDefault(x => x.IsActive);
            }
            else
            {
                var attComb = _context.AttributeCombinations.Where(x => x.IsActive);
                list.AddRange(attComb.Where(x => x.ProductId == Id && x.IsActive == true));
                var grp = (from com in list
                 select new
                 {
                     Id = list[0].Id,
                     ProductId = list[0].ProductId,
                     Stocks = list[0].Stocks.Where(x => x.IsActive).ToList()
                 }).FirstOrDefault();

                if (grp == null)
                {
                    return null;
                }
                return grp.Stocks.FirstOrDefault(x => x.IsActive);
            }
            
            
        }
    }
}
