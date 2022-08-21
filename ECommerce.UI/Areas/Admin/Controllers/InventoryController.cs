using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using Ecommerce.BLL.Handler;
using ECommerce.UI.Models;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class InventoryController : Controller
    {
        EcommerceDBEntities context;
        ProductHandler productHandler;
        AttributeHandler attributeHandler;
        StockHandler stockHandler;
        public InventoryController()
        {
            context = new EcommerceDBEntities();
            productHandler = new ProductHandler(context);
            attributeHandler = new AttributeHandler(context);
            stockHandler = new StockHandler(context);
        }
        // GET: Admin/Inventory
        public ActionResult Index()
        {
            var products = productHandler.List;
            return View(products);
        }

        public ActionResult Add(long Id, long ProdId)
        {
            ViewBag.Attributes = attributeHandler.GetByCategoryId(Id);
            ViewBag.ProdId = ProdId;
            return PartialView("_AddPartial");
        }
        [HttpPost]
        public ActionResult Add(StockCollectionViewModel model, long ProdId)
        {
            List<Stock> stockList = new List<Stock>();
            if (model != null && model.Stocks != null && model.Stocks.Count() > 0)
            {
                foreach (var item in model.Stocks)
                {
                    AttributeCombination atrib = new AttributeCombination();
                    atrib.CreatedOn = DateTime.UtcNow;
                    atrib.IsActive = true;
                    atrib.ProductId = ProdId;

                    Stock stck = new Stock();
                    stck.IsActive = true;
                    stck.CreatedOn = DateTime.UtcNow;
                    stck.Quantity = item.Quantity;
                    stck.ProductId = ProdId;

                    foreach (var comb in item.CombinationDetail)
                    {
                        comb.CreatedOn = DateTime.UtcNow;
                        comb.IsActive = true;
                        atrib.AttribCombinationDetails.Add(comb);
                    }
                    var stckrslt = stockHandler.Find(ProdId, atrib.AttribCombinationDetails.ToList());
                    if (stckrslt != null && stckrslt.Id > 0)
                    {
                        stckrslt.Quantity += item.Quantity;
                        stockHandler.Update(stckrslt);
                    }
                    else
                    {
                        stck.AttributeCombination = atrib;
                        stockHandler.Add(stck);
                    }
                }
            }
            return RedirectToAction("Index");
        }


    }
}