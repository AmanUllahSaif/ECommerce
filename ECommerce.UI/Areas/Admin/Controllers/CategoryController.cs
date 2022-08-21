using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using Ecommerce.BLL.Handler;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private CategoryHandler categoryHandler;
        EcommerceDBEntities context;
        public CategoryController()
        {
            context = new EcommerceDBEntities();
            categoryHandler = new CategoryHandler(context);
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = categoryHandler.NavigationHeadings();
            return View(categories);
        }

        public PartialViewResult Edit(long Id)
        {
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            ViewBag.NavigationCategory = categoryHandler.NavigationCategory();
            var category = categoryHandler.FindById(Id);
            return PartialView("_EditPartial", category);
        }

        [HttpPost]
        public ActionResult Edit(Category model)
        {
            var categroy = categoryHandler.FindById(model.Id);
            categroy.ModifiedOn = DateTime.UtcNow;
            categroy.Name = model.Name;
            categroy.ParentId = model.ParentId;
            categroy.Type = model.Type;
            categroy.IsAdultAlert = model.IsAdultAlert;
            categoryHandler.Update(categroy);
            return RedirectToAction("index");
        }

        public PartialViewResult Create()
        {
            ViewBag.NavigationHeading = categoryHandler.NavigationHeadings();
            ViewBag.NavigationCategory = categoryHandler.NavigationCategory();
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;
            categoryHandler.Add(model);
            return RedirectToAction("index");
        }

        public JsonResult GetNavCategories(long Id)
        {
            var category = categoryHandler.NavigationCategory().Where(x => x.ParentId == Id);
            if (category != null && category.Count() > 0)
            {
                var result = from c in category select new { Id = c.Id, Name = c.Name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetLinks(long Id)
        {
            var links = categoryHandler.NavigationLinks().Where(x => x.ParentId == Id);
            if (links != null && links.Count() > 0)
            {
                var result = from c in links select new { Id = c.Id, Name = c.Name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(long Id)
        {
            var category = categoryHandler.FindById(Id);
            category.ModifiedOn = DateTime.UtcNow;
            category.IsActive = false;
            categoryHandler.Update(category);
            return RedirectToAction("index", "category");
        }
    }
}