using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using Ecommerce.BLL.Handler;
using ECommerce.UI.Models;
using ECommerce.UI.Util;
using System.Transactions;
using Ecommerce.DAL.Enums;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using VNS.Accounts;
using VNS.Accounts.DAL;
using DotNetShipping;
using DotNetShipping.ShippingProviders;

namespace ECommerce.UI.Controllers
{
    public class ShoppingController : Controller
    {
        private ProductHandler productHandler;
        private OrderHandler orderHandler;
        private StockHandler stockHandler;
        private SalesManager salesManager;
        private VNSAccountsDBEntities accountContext;
        private CustomShirtHandler customShirtHandler;
        private EcommerceDBEntities context;

        int PageSize;
        public ShoppingController()
        {
            context = new EcommerceDBEntities();
            accountContext = new VNSAccountsDBEntities();
            productHandler = new ProductHandler(context);
            orderHandler = new OrderHandler(context);
            stockHandler = new StockHandler(context);
            salesManager = new SalesManager(accountContext);
            customShirtHandler = new CustomShirtHandler(context);
            PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        }
        // GET: Shopping
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products(long? Id, string price, string discount, string search)
        {
            ViewBag.id = Id;
            var products = productHandler.List;
            if (Id != null)
            {
                products = products.Where(x => x.CategoryId == Id.GetValueOrDefault());
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(x => x.Description.ToLower().Contains(search.ToLower()) || x.Name.ToLower().Contains(search.ToLower()));
            }
            if (!string.IsNullOrEmpty(price))
            {
                var lstPrice = price.Split(',').ToList(); ;
                lstPrice.Remove(lstPrice.Last());
                if (lstPrice.Count() > 1)
                {
                    decimal start = Convert.ToDecimal(lstPrice.First().Split('-').First());
                    decimal end = Convert.ToDecimal(lstPrice.Last().Split('-').Last());
                    products = products.Where(x => x.Price >= start && x.Price <= end);
                }
                else
                {
                    decimal start = Convert.ToDecimal(lstPrice.First().Split('-').First());
                    decimal end = Convert.ToDecimal(lstPrice.First().Split('-').Last());
                    products = products.Where(x => x.Price >= start && x.Price <= end);
                }
            }

            if (!string.IsNullOrEmpty(discount))
            {
                var lstdiscount = discount.Split(',').ToList(); ;
                lstdiscount.Remove(lstdiscount.Last());
                if (lstdiscount.Count() > 1)
                {
                    decimal start = Convert.ToDecimal(lstdiscount.First().Split('-').First());
                    decimal end = Convert.ToDecimal(lstdiscount.Last().Split('-').Last());
                    products = products.Where(x => x.Discount >= start && x.Discount <= end);
                }
                else
                {
                    decimal start = Convert.ToDecimal(lstdiscount.First().Split('-').First());
                    decimal end = Convert.ToDecimal(lstdiscount.First().Split('-').Last());
                    products = products.Where(x => x.Discount >= start && x.Discount <= end);
                }
            }

            return View(products);
        }

        public ActionResult ProductDetail(long Id)
        {
            ProductDetailViewModel model = new ProductDetailViewModel();
            model.Item = productHandler.FindById(Id);
            model.Item.ProductImages = model.Item.ProductImages.Where(x => x.IsActive).ToList();
            return View(model);
        }

        public ActionResult Checkout()
        {
            var items = CartManager.GetAll();
            ViewBag.total = CartManager.GetAmount();
            return View(items);
        }

        public ActionResult ShippingInfo()
        {
            List<CartViewModel> lst = CartManager.GetAll();
            List<CustomShirtsOrder> lstOrder = CartManager.GetCustomShirts();
            if ((lst == null || lst.Count < 1) && (lstOrder == null || lstOrder.Count < 1))
            {
                return RedirectToAction("Checkout", "shopping");
            }
            var shipping = CartManager.GetShippingInfo();

            return View(shipping);
        }
        [HttpPost]
        public ActionResult ShippingInfo(Order order)
        {
            CartManager.ShippingInfo(order);
            return RedirectToAction("Payment", "Shopping");
        }
        public ActionResult Payment()
        {
            List<CartViewModel> lst = CartManager.GetAll();
            List<CustomShirtsOrder> lstOrder = CartManager.GetCustomShirts();
            if ((lst == null || lst.Count < 1) && (lstOrder == null || lstOrder.Count < 1))
            {
                return RedirectToAction("Checkout", "shopping");
            }
            //if (lst.FirstOrDefault().ShippingInfo == null)
            //{
            //    return RedirectToAction("ShippingInfo", "shopping");
            //}
            return View();
        }

        [HttpPost]
        public ActionResult Payment(CreditCardViewModel model)
        {
            Order order = new Order();

            List<CartViewModel> lst = new List<CartViewModel>();
            lst = CartManager.GetAll();
            var custmShirts = CartManager.GetCustomShirts();
            if (lst != null && lst.Count > 0)
            {
                order = lst.FirstOrDefault().ShippingInfo;
            }
            else
            {
                order = custmShirts.FirstOrDefault().Order;
            }
            decimal price = 0;

            using (TransactionScope trnsScope = new TransactionScope())
            {
                if (custmShirts != null && custmShirts.Count > 0)
                {
                    foreach (var item in custmShirts)
                    {
                        item.CreatedOn = DateTime.UtcNow;
                        item.IsActive = true;
                        order.CustomShirtsOrders.Add(item);
                    }

                    price += order.CustomShirtsOrders.Sum(x => (x.FrontPrice.GetValueOrDefault() + x.BackPrice.GetValueOrDefault() + x.ShirtPrice) * x.Quantity);
                }

                if (User.Identity.IsAuthenticated && User.IsInRole("Bussines"))
                {
                    order.Type = (int)OrderType.B2B;
                    foreach (var item in lst)
                    {
                        OrderDetail detail = new OrderDetail();
                        detail.CreatedOn = DateTime.UtcNow;
                        detail.IsActive = true;
                        detail.ProductId = item.Product.Id;
                        detail.Quantity = item.Quantity;
                        detail.StockId = item.StockId;
                        detail.Price = item.PurchasePrice;
                        Stock stck = new Stock();
                        stck = stockHandler.FindById(item.StockId);
                        if (stck.Quantity >= detail.Quantity)
                        {
                            order.OrderDetails.Add(detail);
                            stck.Quantity -= detail.Quantity;
                            stck.ModifiedOn = DateTime.UtcNow;
                            stockHandler.Update(stck);
                        }
                        else
                        {
                            return RedirectToAction("Checkout", "Shopping", new { message = "Item '" + item.Product.Name + "' out of stock" });
                        }
                    }
                    price += order.B2BDetail.Sum(x => x.Price * x.PackageQuantity);
                }
                else
                {
                    order.Type = (int)OrderType.Genral;

                    foreach (var item in lst)
                    {
                        OrderDetail detail = new OrderDetail();
                        detail.CreatedOn = DateTime.UtcNow;
                        detail.IsActive = true;
                        detail.ProductId = item.Product.Id;
                        detail.Quantity = item.Quantity;
                        detail.StockId = item.StockId;
                        if (item.Product.Sale)
                        {
                            detail.Price = item.Product.Price - (item.Product.Price * (item.Product.Discount.Value / 100));
                        }
                        else
                        {
                            detail.Price = item.Product.Price;
                        }
                        Stock stck = new Stock();
                        stck = stockHandler.FindById(item.StockId);
                        if (stck.Quantity >= detail.Quantity)
                        {
                            order.OrderDetails.Add(detail);
                            stck.Quantity -= detail.Quantity;
                            stck.ModifiedOn = DateTime.UtcNow;
                            stockHandler.Update(stck);
                        }
                        else
                        {
                            return RedirectToAction("Checkout", "Shopping", new { message = "Item '" + item.Product.Name + "' out of stock" });
                        }
                    }
                    price += order.OrderDetails.Sum(x => x.Price * x.Quantity);
                }

                order.CreatedOn = DateTime.UtcNow;
                order.IsActive = true;
                order.UserId = User.Identity.GetCustomId();
                orderHandler.Add(order);
                //if (User.Identity.IsAuthenticated)
                //{
                //    salesManager.CreateSale(order.Id, price, DateTime.UtcNow, User.Identity.GetCustomId());
                //}
                //else
                //{
                //    salesManager.CreateSale(order.Id, price, DateTime.UtcNow);
                //}
                PaymentManager.Pay(lst, price, model, order.Id);
                PaymentManager.Refund("PAY-35V09599158467548LIID5UQ", price);
                EmailManager.SendNewOrderEmail(order.Id, order.Email, CartManager.GetAmount());
                CartManager.Clear();
                trnsScope.Complete();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Orders(int page = 1)
        {
            var ordrs = orderHandler.List.Where(x => x.UserId == User.Identity.GetCustomId()).OrderByDescending(x => x.CreatedOn).ToPagedList(page, PageSize);
            return View(ordrs);
        }


        public JsonResult GetPrice()
        {
            // You will need an account # and meter # to utilize the FedEx provider.
            string fedexKey = "ZC8EV9jUZ6nnBBFH";
            string fedexPassword = "Smoke123";
            string fedexAccountNumber = "510087240";
            string fedexMeterNumber = "100336830";

            // You will need a userId to use the USPS provider. Your account will also need access to the production servers.
            string uspsUserId = "966URZTR7810";

            // Setup package and destination/origin addresses
            var packages = new List<DotNetShipping.Package>();
            packages.Add(new DotNetShipping.Package(12, 12, 12, 35, 150, null, true));
            var shipinginfo = CartManager.GetShippingInfo();
            var origin = new Address("HOUSTAN", "TX", "77036", "US");
            var destination = new Address(shipinginfo.City, "", shipinginfo.PostalCode, shipinginfo.Country); // US Address

            // Create RateManager            var rateManager = new RateManager();

            // Add desired DotNetShippingProviders
            // rateManager.AddProvider(new UPSProvider(upsLicenseNumber, upsUserId, upsPassword));
            //ateManager.AddProvider(new FedExProvider(fedexKey, fedexPassword, fedexAccountNumber, fedexMeterNumber));
            //rateManager.AddProvider(new USPSProvider(uspsUserId));

            // (Optional) Add RateAdjusters
            //rateManager.AddRateAdjuster(new PercentageRateAdjuster(.9M));

            // Call GetRates()
            // Shipment shipment = rateManager.GetRates(origin, destination, packages);

            // Iterate through the rates returned
            //foreach (Rate rate in shipment.Rates)
            //{
            //    Console.WriteLine(rate);
            //}
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Shirts()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Shirts(CustomShirtsOrder model)
        {
            if (!string.IsNullOrEmpty(model.BackImageUrl))
            {
                model.BackImageUrl = FileManager.SaveBase64Img(model.BackImageUrl);
            }
            if (!string.IsNullOrEmpty(model.FrontImageUrl))
            {
                model.FrontImageUrl = FileManager.SaveBase64Img(model.FrontImageUrl);
            }
            CartManager.AddCustomShirt(model);
            return View();
        }

        public PartialViewResult CustomShirtsCheckOut()
        {
            var cshirts = CartManager.GetCustomShirts();
            return PartialView(cshirts);
        }

        public JsonResult GetShirtDetail(int Id)
        {
            var model = customShirtHandler.GetByType(Id);
            if (model == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            switch (model.Type)
            {
                case (int)ShirtType.ShortSleeveRound:
                    model.FrontImageUrl = "/CustomShirts/crew_front.png";
                    model.BackImageUrl = "/CustomShirts/crew_back.png";
                    break;
                case (int)ShirtType.ShortSleeveV:
                    model.FrontImageUrl = "/CustomShirts/half-sleeve-V.png";
                    model.BackImageUrl = "/CustomShirts/crew_back.png";
                    break;
                //case (int)ShirtType.LongSleeveRound:
                //    model.FrontImageUrl = "/CustomShirts/long-sleeve-V.png";
                //    model.BackImageUrl = "/CustomShirts/long_back.png";
                //    break;
                default:
                    break;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}