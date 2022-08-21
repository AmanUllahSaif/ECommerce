using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using ECommerce.UI.Util;
using ECommerce.UI.Models;
using System.Configuration;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pagesize"]);
        EcommerceDBEntities context;
        ProductHandler productHandler;
        CategoryHandler categoryHandler;
        AttributeHandler attributeHandler;
        public ProductController()
        {
            context = new EcommerceDBEntities();
            productHandler = new ProductHandler(context);
            categoryHandler = new CategoryHandler(context);
            attributeHandler = new AttributeHandler(context);
        }
        // GET: Admin/Product
        public ActionResult Index(int page = 1)
        {
            var products = productHandler.List.OrderByDescending(x => x.CreatedBy).ToPagedList(page, pageSize); ;
            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = categoryHandler.NavigationLinks();
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            return View();
        }
        [HttpPost]
        public ActionResult Save(Product model, IEnumerable<HttpPostedFileBase> productimage, StockCollectionViewModel stock)
        {
            List<ProductImage> images = new List<ProductImage>();
            if (productimage != null)
            {
                foreach (var item in productimage)
                {
                    string fileurl = FileManager.SaveImage(item);
                    ProductImage img = new ProductImage();
                    img.Url = fileurl;
                    img.CreatedOn = DateTime.UtcNow;
                    img.IsActive = true;

                    images.Add(img);
                }
            }
            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;
            model.ProductImages = images;

            List<Stock> stockList = new List<Stock>();
            if (stock != null && stock.Stocks != null && stock.Stocks.Count() > 0)
            {
                foreach (var item in stock.Stocks)
                {
                    AttributeCombination atrib = new AttributeCombination();
                    atrib.CreatedOn = DateTime.UtcNow;
                    atrib.IsActive = true;

                    Stock stck = new Stock();
                    stck.IsActive = true;
                    stck.CreatedOn = DateTime.UtcNow;
                    stck.Quantity = item.Quantity;
                    if (item.CombinationDetail != null)
                    {
                        foreach (var comb in item.CombinationDetail)
                        {
                            comb.CreatedOn = DateTime.UtcNow;
                            comb.IsActive = true;
                            atrib.AttribCombinationDetails.Add(comb);
                        }
                    }

                    stck.AttributeCombination = atrib;
                    stockList.Add(stck);
                }
            }

            model.Stocks = stockList;
            productHandler.Add(model);

            return RedirectToAction("index");
        }

        public ActionResult Edit(long Id)
        {
            var product = productHandler.FindById(Id);
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            ViewBag.Categories = categoryHandler.NavigationLinks();
            ViewBag.images = product.ProductImages.Where(x => x.IsActive);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product model, List<long> imageId, List<HttpPostedFileBase> productimage)
        {
            var oldModel = productHandler.FindById(model.Id);
            oldModel.ModifiedOn = DateTime.UtcNow;
            oldModel.Name = model.Name;
            oldModel.Price = model.Price;
            oldModel.Description = model.Description;
            oldModel.Discount = model.Discount;
            oldModel.Featured = model.Featured;
            oldModel.CategoryId = model.CategoryId;
            oldModel.Sale = model.Sale;
            foreach (var item in oldModel.ProductImages)
            {
                item.IsActive = false;
                item.ModifiedOn = DateTime.UtcNow;
                item.ModifiedBy = User.Identity.GetCustomId();
            }

            for (int i = 0; i < productimage.Count; i++)
            {

                if (imageId[i] > 0 && productimage[i] != null)
                {
                    var oldImage = oldModel.ProductImages.SingleOrDefault(x => x.Id == imageId[i]);
                    oldImage.IsActive = true;
                    oldImage.Url = FileManager.SaveImage(productimage[i]);
                }
                if (imageId[i] == 0 && productimage[i] != null)
                {
                    var oldImage = new ProductImage();
                    oldImage.IsActive = true;
                    oldImage.CreatedOn = DateTime.UtcNow;
                    oldModel.CreatedBy = User.Identity.GetCustomId();
                    oldImage.Url = FileManager.SaveImage(productimage[i]);
                    oldModel.ProductImages.Add(oldImage);
                }
                if (imageId[i] > 0 && productimage[i] == null)
                {
                    var oldImage = oldModel.ProductImages.SingleOrDefault(x => x.Id == imageId[i]);
                    oldImage.IsActive = true;
                    oldImage.ModifiedOn = DateTime.UtcNow;
                    oldImage.ModifiedBy = User.Identity.GetCustomId();
                }
            }

            productHandler.Update(oldModel);
            return RedirectToAction("index", "Product");
        }

        public PartialViewResult Listing(long Id)
        {
            ViewBag.Attributes = attributeHandler.GetByCategoryId(Id);
            return PartialView("_ListingPartial");
        }
        public ActionResult Delete(long id)
        {
            Product product = productHandler.FindById(id);
            product.ProductImages = product.ProductImages.Where(x => x.IsActive).ToList();
            product.ModifiedOn = DateTime.UtcNow;
            product.IsActive = false;

            //foreach (var item in product.ProductImages)
            //{
            //    item.IsActive = false;
            //    item.ModifiedOn = DateTime.UtcNow;
            //    item.ModifiedBy = User.Identity.GetCustomId();
            //}
            productHandler.Update(product);
            return RedirectToAction("index");

        }

        public JsonResult ProductsByCategory(long Id)
        {
            var prods = productHandler.List.OrderByDescending(x => x.CreatedOn);
            if (prods != null && prods.Count() > 0)
            {
                var jsonProds = from prd in prods select new { Id = prd.Id, Name = prd.Name };
                return Json(jsonProds, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

    }
}