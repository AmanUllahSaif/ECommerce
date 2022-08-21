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
    public class BannerController : Controller
    {
        EcommerceDBEntities context;
        BannerHandler BannerHandler;

        public BannerController()
        {
            context = new EcommerceDBEntities();
            BannerHandler = new BannerHandler(context);
        }
        // GET: Admin/Banner
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CreateBanner()
        {
            return PartialView("_CreatePartial");
        }
        public ActionResult Save(Banner model, HttpPostedFileBase BannerImage)
        {
            if (BannerImage != null)
            {
                string FileUrl = FileManager.SaveImage(BannerImage);
                model.ImgUrl = FileUrl;
            }
            model.CreatedOn = DateTime.UtcNow;
            model.IsActive = true;
            BannerHandler.Add(model);
            return RedirectToAction("index");
        }
        public PartialViewResult Edit(long id)
        {
            Banner banner = BannerHandler.FindById(id);
            return PartialView("_EditPartial", banner);
        }
        public ActionResult Update(long id,Banner model, HttpPostedFileBase BannerImage)
        {
            Banner oldBanner = BannerHandler.FindById(id);
            oldBanner.Heading = model.Heading;
            oldBanner.Description = model.Description;
            oldBanner.AnimationType = model.AnimationType;
            if (BannerImage != null)
            {
                string FileUrl = FileManager.SaveImage(BannerImage);
                oldBanner.ImgUrl = FileUrl;
            }
            BannerHandler.Update(oldBanner);
            return RedirectToAction("index");
        }
        public PartialViewResult ListBanner()
        {
            var Banners = BannerHandler.List;
            return PartialView("_ListBannerPartial", Banners);
        }
        public ActionResult Delete(long id)
        {
            Banner Banner= BannerHandler.FindById(id);
            Banner.ModifedOn = DateTime.UtcNow;
            BannerHandler.Delete(Banner);
            return RedirectToAction("index");
        }
    }
}