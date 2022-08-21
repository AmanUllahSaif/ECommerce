using Ecommerce.BLL.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using Ecommerce.DAL.Enums;
using ECommerce.UI.Util;
using VNS.Accounts.DAL;
using VNS.Accounts;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        private OrderHandler orderHandler;
        private EcommerceDBEntities context;
        private VNSAccountsDBEntities accountContext;
        private SalesManager saleManager;

        public OrderController() {
            context = new EcommerceDBEntities();
            orderHandler = new OrderHandler(context);
            accountContext = new VNSAccountsDBEntities();
            saleManager = new SalesManager(accountContext);
            
        }
        // GET: Admin/Order
        public ActionResult Index()
        {
            var orders = orderHandler.List;
            return View(orders);
        }

        public ActionResult Orders(int page = 1)
        {
            var orders = orderHandler.List.Where(x => x.Type == (int)OrderType.Genral);
            return PartialView("_OrdersPartial", orders);
        }

        public ActionResult B2BOrders(int page = 1)
        {
            var orders = orderHandler.List.Where(x => x.Type == (int)OrderType.B2B);
            return PartialView("_B2BOrdersPartial", orders);
        }
        public ActionResult Detail(long Id)
        {
            Order order = new Order();
            order = orderHandler.FindById(Id);
            return View(order);
        }

        public ActionResult B2BDetail(long Id)
        {
            Order order = new Order();
            order = orderHandler.FindById(Id);
            return View(order);
        }
        public ActionResult ChangeStatus(long Id, int status)
        {
            var order = orderHandler.FindById(Id);
            order.ModifiedOn = DateTime.UtcNow;
            order.Status = status;
            if ((OrderStatus)status == OrderStatus.Rejected || (OrderStatus)status == OrderStatus.Returned)
            {
                orderHandler.Return(Id);
                decimal total = 0;
                if (order.Type == (int)OrderType.B2B)
                {
                    total += order.B2BDetail.Sum(x => x.Price * x.PackageQuantity * x.Quantity);
                }
                else
                {
                    total += order.OrderDetails.Where(x => x.IsActive).Sum(x => x.Price * x.Quantity);
                }
                if (order.CustomShirtsOrders != null && order.CustomShirtsOrders.Count > 0)
                {
                    total += order.CustomShirtsOrders.Where(x => x.IsActive).Sum(x => (x.BackPrice.GetValueOrDefault() + x.FrontPrice.GetValueOrDefault() + x.ShirtPrice) * x.Quantity);
                }
                saleManager.ReturnSale(Id, total, DateTime.UtcNow);
            }
            switch (status)
            {
                case (int)OrderStatus.Inprocess:
                    EmailManager.SendInprocessMail(order.Id, order.Email);
                    break;
                case (int)OrderStatus.Rejected:
                    EmailManager.SendRejectMail(order.Id, order.Email);
                    break;
                case (int)OrderStatus.Returned:
                    EmailManager.SendOrderReturnedMail(order.Id, order.Email);
                    break;
                case (int)OrderStatus.Shipped:
                    EmailManager.SendOrderShippedMail(order.Id, order.Email);
                    break;
                default:
                    break;
            }
            orderHandler.Update(order);

            var orders = orderHandler.List;
            return PartialView("_OrdersPartial", orders);
        }
    }
}