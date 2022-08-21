using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNS.Accounts.DAL;

namespace ECommerce.UI.Areas.Admin.Models
{
    public class IncomeStatementViewModel
    {
        public IEnumerable<Account> Revinue { get; set; }
        public IEnumerable<Account> Expense { get; set; }
    }
}