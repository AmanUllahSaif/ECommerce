using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecommerce.DAL;
using Ecommerce.BLL.Handler;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using VNS.Accounts;
using VNS.Accounts.DAL;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class VendorController : Controller
    {
        EcommerceDBEntities context;
        VendorHandler vendorHandler;
        VNSAccountsDBEntities accountContext;
        AccountsManager accountManager;
        int pageSize;
        public VendorController()
        {
            context = new EcommerceDBEntities();
            vendorHandler = new VendorHandler(context);
            pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pagesize"]);
            accountContext = new VNSAccountsDBEntities();
            accountManager = new AccountsManager(accountContext);
        }

        // GET: Admin/Vendor
        public ActionResult Index(int page =1)
        {
            var vendors = vendorHandler.List.OrderByDescending(x => x.CreatedOn).ToPagedList(page, pageSize);
            return View(vendors);
        }

        public PartialViewResult Create()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        public ActionResult Create(Vendor model)
        {
            model.IsActive = true;
            model.CreatedOn = DateTime.UtcNow;
            vendorHandler.Add(model);
            accountManager.CreateAccount(model.Name + " Account", 0, false, AccountType.Libilites, null, model.Id);
            return RedirectToAction("index");
        }

        public ActionResult Delete(long Id)
        {
            var vendor = vendorHandler.FindById(Id);
            vendor.ModifiedOn = DateTime.UtcNow;
            vendorHandler.Delete(vendor);
            return RedirectToAction("Index");
        }

        public PartialViewResult Edit(long Id)
        {
            var vendor = vendorHandler.FindById(Id);
            return PartialView("_EditPartial", vendor);
        }

        [HttpPost]
        public ActionResult Edit(Vendor model)
        {
            var oldVendor = vendorHandler.FindById(model.Id);
            oldVendor.Name = model.Name;
            oldVendor.Email = model.Email;
            oldVendor.Contact = model.Contact;
            oldVendor.ModifiedOn = DateTime.UtcNow;
            vendorHandler.Update(oldVendor);
            return RedirectToAction("index");
        }
    }
}