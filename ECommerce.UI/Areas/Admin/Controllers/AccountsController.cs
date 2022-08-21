using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNS.Accounts.DAL;
using VNS.Accounts;
using ECommerce.UI.Areas.Admin.Models;

namespace ECommerce.UI.Areas.Admin.Controllers
{
    public class AccountsController : Controller
    {
        VNSAccountsDBEntities vnsAccountsContext;
        AccountsManager accountsManager;

        public AccountsController()
        {
            vnsAccountsContext = new VNSAccountsDBEntities();
            accountsManager = new AccountsManager(vnsAccountsContext);
        }
        // GET: Admin/Accounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeneralJournal(DateTime? from , DateTime? to)
        {
            if (from == null && to == null)
            {
                var todaytrns = accountsManager.GetTodayTransactions();
                return View(todaytrns);
            }
            var trns = accountsManager.GetTransaction(from, to);
            return View(trns);
        }

        public ActionResult IncomeStatement()
        {
            IncomeStatementViewModel model = new IncomeStatementViewModel();
            model.Expense = accountsManager.GetExpense(null, null);
            model.Revinue = accountsManager.GetRevenue(null, null);
            //var expense 
            return View(model);
        }
    }
}