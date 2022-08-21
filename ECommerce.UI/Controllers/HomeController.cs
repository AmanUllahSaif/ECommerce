using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.BLL;
using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using Ecommerce.Repo.Handler;

namespace ECommerce.UI.Controllers
{
    public class HomeController : Controller
    {
        private CategoryHandler categoryHandler;
        private EcommerceDBEntities context;
        private ProductHandler prodcutHandler;
        private BannerHandler bannerHandler;
        private DiscountBannerHandler discountBannerHandler;

        public HomeController()
        {
            context = new EcommerceDBEntities();
            categoryHandler = new CategoryHandler(context);
            prodcutHandler = new ProductHandler(context);
            bannerHandler = new BannerHandler(context);
            discountBannerHandler = new DiscountBannerHandler(context);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Refund()
        {
            return View();
        }

        public PartialViewResult NavBar()
        {
            var Category = categoryHandler.NavigationHeadings();
            return PartialView("_NavBarPartial", Category);
        }
        public PartialViewResult Banner()
        {
            var lst = bannerHandler.List;
            return PartialView("_BannerPartial", lst);
        }
        public PartialViewResult DiscountBanner()
        {
            var lst = discountBannerHandler.List;
            return PartialView("_DiscountBannerPartial", lst);
        }
        public PartialViewResult FeaturedProducts()
        {
            var products = prodcutHandler.GetFeatured();
            return PartialView("_FeaturedProductsPartial", products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}