using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using PagedList;
using PagedList.Mvc;
using Ecommerce.BLL.Handler;
using System.Configuration;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PackageController : Controller
    {
        EcommerceDBEntities context;
        PackageHandler packageHandler;
        ProductHandler productHandler;
        int pageSize;
        public PackageController()
        {
            context = new EcommerceDBEntities();
            packageHandler = new PackageHandler(context);
            productHandler = new ProductHandler(context);
            pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        }
        // GET: Admin/Package
        public ActionResult Index(int page = 1)
        {
            var lst = packageHandler.List.ToPagedList(page, pageSize);
            return View(lst);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            ViewBag.products = productHandler.List;
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        public ActionResult Create(Package model)
        {
            model.CreatedOn = DateTime.UtcNow;
            model.IsActive = true;
            packageHandler.Add(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(long Id)
        {
            var model = packageHandler.FindById(Id);
            model.IsActive = false;
            model.ModifiedOn = DateTime.UtcNow;
            packageHandler.Delete(model);
            return RedirectToAction("index");
        }

        [HttpGet]
        public PartialViewResult Edit(long Id)
        {
            var model = packageHandler.FindById(Id);
            ViewBag.products = productHandler.List;
            return PartialView("_EditPartial", model);
        }

        [HttpPost]
        public ActionResult Edit(Package model)
        {
            var oldmodel = packageHandler.FindById(model.Id);
            oldmodel.ModifiedOn = DateTime.UtcNow;
            oldmodel.Price = model.Price;
            oldmodel.ProductId = model.ProductId;
            oldmodel.StartQuantity = model.StartQuantity;
            oldmodel.EndQuantity = model.EndQuantity;
            packageHandler.Update(oldmodel);
            return RedirectToAction("index");
        }
    }
}