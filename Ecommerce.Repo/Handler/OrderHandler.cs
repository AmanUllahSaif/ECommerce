using Ecommerce.DAL;
using Ecommerce.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ecommerce.BLL.Handler
{
    public class OrderHandler : IGenericRepository<Order>
    {
        private EcommerceDBEntities _context;
        public OrderHandler(EcommerceDBEntities context)
        {
            _context = context;
        }

        public IEnumerable<Order> List
        {
            get
            {
                return _context.Orders.Where(x => x.IsActive == true).OrderByDescending(x=>x.CreatedOn);
            }
        }

        public void Add(Order model)
        {
            _context.Orders.Add(model);
            _context.SaveChanges();
        }

        public void Update(Order model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Order entity)
        {
            entity.IsActive = false;
            this.Update(entity);

        }
        public Order FindById(long Id)
        {
            return _context.Orders.Find(Id);
        }

        public void Return(long Id)
        {
            using (TransactionScope trnsScope = new TransactionScope())
            {
                var order = _context.Orders.Find(Id);
                if (order.Type == (int)OrderType.B2B)
                {
                    foreach (var item in order.B2BDetail)
                    {
                        var stock = _context.Stocks.Find(item.StockId);
                        stock.Quantity += (item.Quantity * item.PackageQuantity);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    foreach (var item in order.OrderDetails)
                    {
                        var stock = _context.Stocks.Find(item.StockId);
                        stock.Quantity += item.Quantity;
                        _context.SaveChanges();
                    }
                }
                trnsScope.Complete();
            }
            
        }
    }
}
