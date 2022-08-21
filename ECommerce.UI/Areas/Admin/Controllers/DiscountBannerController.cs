using Ecommerce.DAL;
using Ecommerce.Repo.Handler;
using ECommerce.UI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DiscountBannerController : Controller
    {
        EcommerceDBEntities context;
        DiscountBannerHandler DiscountBannerHandler;

        public DiscountBannerController()
        {
            context = new EcommerceDBEntities();
            DiscountBannerHandler = new DiscountBannerHandler(context);
        }
        // GET: Admin/DiscountBanner
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CreateDiscountBanner()
        {
            return PartialView("__CreatePartial");
        }
        public PartialViewResult ListDiscountBannerPartial()
        {
            var DiscountBannner = DiscountBannerHandler.List;
            return PartialView("_ListDiscountBannerPartial", DiscountBannner);
        }
        [HttpPost]
        public ActionResult Save(SaleBanner model,HttpPostedFileBase DiscountBannerImage)
        {
            if (DiscountBannerImage != null)
            {
                string FileUrl = FileManager.SaveImage(DiscountBannerImage);
                model.ImageUrl = FileUrl;
            }
            model.CreatedOn = DateTime.UtcNow;
            model.IsActive = true;
            DiscountBannerHandler.Add(model);
            return RedirectToAction("index");
        }
        public PartialViewResult Edit(long id)
        {
            SaleBanner saleBanner = DiscountBannerHandler.FindById(id);
            return PartialView("_EditPartial",saleBanner);
        }
        [HttpPost]
        public ActionResult Update(long id,SaleBanner model,HttpPostedFileBase DiscountBannerImage)
        {
            SaleBanner oldSaleBanner = DiscountBannerHandler.FindById(id);
            oldSaleBanner.Text = model.Text;
            oldSaleBanner.PageUrl = model.PageUrl;
            oldSaleBanner.TextColor = model.TextColor;
            if(DiscountBannerImage != null)
            {
                string FileUrl = FileManager.SaveImage(DiscountBannerImage);
                oldSaleBanner.ImageUrl = FileUrl;
            }
            DiscountBannerHandler.Update(oldSaleBanner);
            return RedirectToAction("index");
        }
        public ActionResult Delete(long id)
        {
            SaleBanner saleBanner = DiscountBannerHandler.FindById(id);
            saleBanner.ModifiedOn = DateTime.UtcNow;
            DiscountBannerHandler.Delete(saleBanner);
            return RedirectToAction("index");
        }
    }
}