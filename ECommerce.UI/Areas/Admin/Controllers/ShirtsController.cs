using Ecommerce.DAL;
using Ecommerce.Repo.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using Ecommerce.BLL.Handler;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ShirtsController : Controller
    {

        EcommerceDBEntities context;
        CustomShirtHandler customShirtHandler;
        int PageSize;
        public ShirtsController()
        {
            context = new EcommerceDBEntities();
            PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            customShirtHandler = new CustomShirtHandler(context);
        }
        // GET: Admin/Shirts
        public ActionResult Index(int page = 1)
        {
            var shirts = customShirtHandler.List.ToPagedList(page, PageSize);
            return View(shirts);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CustomShirt model)
        {
            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;
            customShirtHandler.Add(model);
            return RedirectToAction("index");
        }

        public ActionResult Delete(long Id)
        {
            var shirt = customShirtHandler.FindById(Id);
            shirt.ModifiedOn = DateTime.UtcNow;
            customShirtHandler.Delete(shirt);
            return RedirectToAction("index");
        }
        [HttpGet]
        public ActionResult Edit(long Id)
        {
            var model = customShirtHandler.FindById(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CustomShirt model)
        {
            var oldModel = customShirtHandler.FindById(model.Id);
            oldModel.Type = model.Type;
            oldModel.ShirtPrice = model.ShirtPrice;
            oldModel.FrontPrintPrice = model.FrontPrintPrice;
            oldModel.BackPrintPrice = model.BackPrintPrice;
            oldModel.ModifiedOn = DateTime.UtcNow;
            customShirtHandler.Update(oldModel);
            return RedirectToAction("index");
        }
    }
}