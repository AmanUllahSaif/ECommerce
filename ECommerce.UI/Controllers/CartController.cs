using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.UI.Util;
using ECommerce.UI.Models;
using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using PayPal;

namespace ECommerce.UI.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        ProductHandler productHandler;
        EcommerceDBEntities context;
        StockHandler stockHandler;
        PackageHandler pacakgeHandler;
        public CartController()
        {
            context = new EcommerceDBEntities();
            productHandler = new ProductHandler(context);
            stockHandler = new StockHandler(context);
            pacakgeHandler = new PackageHandler(context);
        }
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CartItems()
        {
            var items = CartManager.GetAll();
            ViewBag.Total = CartManager.GetAmount();
            return PartialView("_CartItemsPartial",items);
        }
        public PartialViewResult CustomShirts()
        {
            var cshirts = CartManager.GetCustomShirts();
            return PartialView("_CustomShirtsPartial", cshirts);
        }

        [HttpPost]
        public RedirectResult Add(ProductDetailViewModel model)
        {
            CartViewModel cModel = new CartViewModel();
            cModel.Product = productHandler.FindById(model.Item.Id);
            cModel.Quantity = model.Quantity;
            Stock stock = stockHandler.Find(model.Item.Id, model.Veriations);
            if (stock != null && stock.Quantity >= model.Quantity)
            {
                cModel.StockId = stock.Id;
                cModel.AttributeCombination = stock.AttributeCombination;
                CartManager.AddItemToCart(cModel);
            }
            else
            {
                ViewBag.message = "Out of Stock.";
                return Redirect(HttpContext.Request.UrlReferrer.AbsolutePath + "?message=Out of Stock.");
            }
            ViewBag.Total = CartManager.GetAmount();
            if (User.Identity.IsAuthenticated && User.IsInRole("Bussines"))
            {
                var gt = CartManager.GetCartItem(model.PacakgeId);
                Package package = new Package();
                if (gt != null && gt.StockId > 0)
                {
                    decimal qty = model.Quantity + gt.Quantity;
                    package = pacakgeHandler.List.FirstOrDefault(x => x.ProductId == cModel.Product.Id && x.EndQuantity >= qty && x.StartQuantity <= qty);
                }
                else
                {
                    package = pacakgeHandler.List.FirstOrDefault(x => x.ProductId == cModel.Product.Id && x.EndQuantity >= model.Quantity && x.StartQuantity <= model.Quantity);
                }
                model.Veriations.ToList();
                cModel.PackageId = package.Id;
                cModel.PurchasePrice = package.Price;
            }
            //if (stock != null && stock.Quantity >= model.Quantity)
            //{
            //    cModel.StockId = stock.Id;
            //    cModel.AttributeCombination = stock.AttributeCombination;

            //    CartManager.AddItemToCart(cModel);
            //}
            //else
            //{
            //    ViewBag.message = "Out of Stock.";
            //    return Redirect(HttpContext.Request.UrlReferrer.AbsolutePath + "?message=Out of Stock.");
            //}
            return Redirect(Url.Content("~/home/index"));
        }

        public PartialViewResult removecustom(int Id)
        {
            CartManager.RemoveCustomShirt(Id);
            var items = CartManager.GetAll();
            ViewBag.Total = CartManager.GetAmount();
            return PartialView("_CartItemsPartial", items);
        }

        public PartialViewResult Remove(long Id)
        {
            CartManager.RemoveItem(Id);
            var items = CartManager.GetAll();
            ViewBag.Total = CartManager.GetAmount();
            return PartialView("_CartItemsPartial", items);
        }
    }
}