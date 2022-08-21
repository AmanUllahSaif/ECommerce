using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using ECommerce.UI.Util;
using ECommerce.UI.Models;
using System.Configuration;
using PagedList;
using VNS.Accounts;
using VNS.Accounts.DAL;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PurchaseController : Controller
    {
        EcommerceDBEntities context;
        StockHandler stockHandler;
        CategoryHandler categoryHandler;
        VendorHandler vendorHandler;
        PurchaseHandler purchaseHandler;
        VNSAccountsDBEntities vnscontext;
        PurchaseManager purchaseManager;
        int PageSize;
        public PurchaseController()
        {
            context = new EcommerceDBEntities();
            stockHandler = new StockHandler(context);
            categoryHandler = new CategoryHandler(context);
            vendorHandler = new VendorHandler(context);
            purchaseHandler = new PurchaseHandler(context);
            vnscontext = new VNSAccountsDBEntities();
            purchaseManager = new PurchaseManager(vnscontext);
            PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        }
        // GET: Admin/Purchase
        public ActionResult Index(int page =1)
        {
            var purchase = purchaseHandler.List.ToPagedList(page, PageSize);
            return View(purchase);
        }
        public ActionResult Create()
        {
            var cart = CartManager.GetAdminAll();
            ViewBag.Vendors = vendorHandler.List;
            return View(cart);
        }
        public ActionResult Detail(long Id)
        {
            Purchase purchase = purchaseHandler.FindById(Id);
            return View(purchase);
        }
        [HttpPost]
        public ActionResult Create(long? Vendore)
        {
            var cart = CartManager.GetAdminAll();
            bool isValid = true;
            if (cart == null)
            {
                ModelState.AddModelError("", "There is no item please add item(s)");
                isValid = false;
            }
            if (Vendore < 1 || Vendore == null)
            {
                ModelState.AddModelError("", "Select a vendor.");
                isValid = false;
            }
            if (!isValid)
            {
                ViewBag.Vendors = vendorHandler.List;
                return View(cart);
            }
            Purchase order = new Purchase();
            order.CreatedOn = DateTime.UtcNow;
            order.IsActive = true;
            order.VendorId = Vendore.GetValueOrDefault();
            foreach (var item in cart)
            {
                PurchaseDetail detail = new PurchaseDetail();
                detail.Price = item.PurchasePrice;
                detail.ProductId = cart[0].Product.Id;
                detail.StockId = item.StockId;
                detail.Quantity = item.Quantity;
                detail.IsActive = true;
                detail.CreatedOn = DateTime.UtcNow;
                order.PurchaseDetails.Add(detail);
                var stck = stockHandler.FindById(item.StockId);
                stck.ModifiedOn = DateTime.UtcNow;
                stck.Quantity += item.Quantity;
                stockHandler.Update(stck);
            }
            purchaseHandler.Add(order);
            CartManager.ClearAdminCart();
            purchaseManager.CreatePurchase(order.Id, order.PurchaseDetails.Sum(x => x.Price * x.Quantity), DateTime.UtcNow);
            return RedirectToAction("Detail", new { Id=order.Id});
        }

        public PartialViewResult Add()
        {
            ViewBag.Categories = categoryHandler.NavigationLinks();
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            return PartialView("_AddPartial");
        }

        [HttpPost]
        public ActionResult Add(StockCollectionViewModel model)
        {
            foreach (var item in model.Stocks)
            {
                CartViewModel cModel = new CartViewModel();
                cModel.Quantity = item.Quantity;
                cModel.PurchasePrice = model.Stocks[0].PurchasePrice;
                Stock stock = stockHandler.Find(model.Stocks[0].ProductId, item.CombinationDetail);
                ViewBag.Total = CartManager.GetAmount();
                if (stock != null )
                {
                    cModel.StockId = stock.Id;
                    cModel.AttributeCombination = stock.AttributeCombination;
                    cModel.Product = stock.Product;
                    CartManager.AddItemToAdminCart(cModel);
                }
                else
                {
                    Stock stck = new Stock();
                    stck.IsActive = true;
                    stck.ProductId = model.Stocks[0].ProductId;
                    stck.Quantity = 0;
                    stck.CreatedOn = DateTime.UtcNow;

                    foreach (var comb in item.CombinationDetail)
                    {
                        comb.IsActive = true;
                        comb.CreatedOn = DateTime.UtcNow;
                    }

                    AttributeCombination attr = new AttributeCombination();
                    attr.IsActive = true;
                    attr.CreatedOn = DateTime.UtcNow;
                    attr.ProductId = model.Stocks[0].ProductId;
                    attr.AttribCombinationDetails = item.CombinationDetail;
                    stck.AttributeCombination = attr;
                    stockHandler.Add(stck);

                    cModel.StockId = stck.Id;
                    cModel.AttributeCombination = stck.AttributeCombination;
                    cModel.Product = stck.Product;
                    CartManager.AddItemToAdminCart(cModel);

                }
            }
            return RedirectToAction("Create");
        }

    }
}