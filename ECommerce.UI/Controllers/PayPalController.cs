using ECommerce.UI.Areas.Admin.Models;
using ECommerce.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.UI.Controllers
{
    public class PayPalController : Controller
    {
        public ActionResult ValidateCommand(string product, string totalPrice, string quantity)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PayPalModel(useSandbox);

            paypal.item_name = product;
            paypal.amount = totalPrice;
            paypal.item_quantity = quantity;
            return View(paypal);
        }

        public ActionResult RedirectFromPaypal()
        {
            return View();
        }

        public ActionResult CancelFromPaypal()
        {
            return View();
        }

        public ActionResult NotifyFromPaypal()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}