using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
    public class AccountsManager
    {
        VNSAccountsDBEntities context;
        public AccountsManager(VNSAccountsDBEntities context)
        {
            this.context = context;
        }
        /// <summary>
        /// This Methode Create a new account.
        /// </summary>
        /// <param name="AccountName">Account Name like Mr.ABC Account.</param>
        /// <param name="openingBalance">Opening Balance of account</param>
        /// <param name="isCr">Is Account Balance in Credit</param>
        /// <param name="type">Nature of account ie. Assets, Libilties</param>
        /// <param name="clientId">If this account belongs to some client</param>
        /// <param name="vndorId">If this account belongs to some Vendor</param>
        public void CreateAccount(string AccountName, decimal openingBalance, bool isCr, AccountType type, long? clientId = null, long? vendorId = null)
        {
            Account acnt = new Account
            {
                Name = AccountName,
                CreatedOn = DateTime.UtcNow,
                Type = (int)type,
                ClientId = clientId,
                VendorId = vendorId
            };
            if (openingBalance > 0)
            {
                AccountDetail detail = new AccountDetail();
                if (isCr)
                {
                    detail.Cr = openingBalance;
                }
                else
                {
                    detail.Dr = openingBalance;
                }
                detail.CreatedOn = DateTime.UtcNow;
                acnt.AccountDetails.Add(detail);
            }
            context.Accounts.Add(acnt);
            context.SaveChanges();
        }

        public IEnumerable<Transaction> GetTodayTransactions()
        {
            return GetTransaction(DateTime.UtcNow, DateTime.UtcNow);
        }
        /// <summary>
        /// Get Transaction between period 
        /// </summary>
        /// <param name="from">Starting date</param>
        /// <param name="to">Ending date</param>
        /// <returns></returns>
        public IEnumerable<Transaction> GetTransaction(DateTime? from, DateTime? to)
        {
            var trns = context.Transactions.Where(x => x.IsActive);
            if (from != null)
            {
                var date = from.GetValueOrDefault();
                trns = trns.Where(x => x.Date.Day >= date.Day && x.Date.Month >= date.Month && x.Date.Year >= date.Year);
            }
            if (to != null)
            {
                var date = to.GetValueOrDefault();
                trns = trns.Where(x => x.Date.Day <= date.Day && x.Date.Month <= date.Month && x.Date.Year <= date.Year);
            }
            return trns;
        }

        public IEnumerable<Account> GetExpense(DateTime? from, DateTime? to)
        {
            var exp = context.Accounts.Where(x => x.IsActive && x.Type == (int)AccountType.Expense);
            
            foreach (var item in exp)
            {
                if (from != null)
                {
                    var date = from.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day >= date.Day && x.Transaction.Date.Month >= date.Month && x.Transaction.Date.Year >= date.Year).ToList();
                }

                if (to != null)
                {
                    var date = to.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day <= date.Day && x.Transaction.Date.Month <= date.Month && x.Transaction.Date.Year <= date.Year).ToList();
                }

                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
            }
            return exp;
        }

        public IEnumerable<Account> GetRevenue(DateTime? from, DateTime? to)
        {
            var reve = context.Accounts.Where(x => x.IsActive && x.Type == (int)AccountType.Revenue);

            foreach (var item in reve)
            {
                if (from != null)
                {
                    var date = from.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day >= date.Day && x.Transaction.Date.Month >= date.Month && x.Transaction.Date.Year >= date.Year).ToList();
                }

                if (to != null)
                {
                    var date = to.GetValueOrDefault();
                    item.AccountDetails = item.AccountDetails.Where(x => x.Transaction.Date.Day <= date.Day && x.Transaction.Date.Month <= date.Month && x.Transaction.Date.Year <= date.Year).ToList();
                }

                item.Balance = item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Dr.GetValueOrDefault()) - item.AccountDetails.Where(x => x.IsActive).Sum(x => x.Cr.GetValueOrDefault());
            }
            return reve;
        }
    }
}
