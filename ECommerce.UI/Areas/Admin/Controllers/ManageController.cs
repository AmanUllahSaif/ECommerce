using Ecommerce.BLL.Handler;
using Ecommerce.DAL;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageController : Controller
    {
        private OrderHandler orderHandler;
        EcommerceDBEntities context;
        private ApplicationUserManager _userManager;
        public ManageController()
        {
            context = new EcommerceDBEntities();
            orderHandler = new OrderHandler(context);
        }
        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Admin/Manage
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult TotalTodayOrders()
        {
            ViewBag.totalOrderOfToday = orderHandler.List.Where(x => x.IsActive).Count();
            return PartialView("_TodayNewOrdersPartialView");
        }

        public PartialViewResult TotalTodayRegisterUser()
        {
            ViewBag.totalUsers = UserManager.Users.Where(x => x.IsApproved).Count();
            return PartialView("_TotalUserRegisterPartialview");
        }
    }
}