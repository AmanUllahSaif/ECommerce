using ECommerce.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
    {
        ApplicationDbContext context;
       public UserManagerController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Admin/UserManager
        public ActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        public ActionResult Disapprove(string email)
        {
            var user = context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
            user.IsApproved = false;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Approve(string email)
        {
            var user = context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
            user.IsApproved = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}