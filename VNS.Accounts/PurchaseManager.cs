using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;
using VNS.Accounts.BLL;

namespace VNS.Accounts
{
    public class PurchaseManager
    {
        TransactionHandler trnsHandler;
        AccountHandler accountHandler;
        public PurchaseManager(VNSAccountsDBEntities context)
        {
            trnsHandler = new TransactionHandler(context);
            accountHandler = new AccountHandler(context);
        }

        public void CreatePurchase(long invoiceId, decimal amount, DateTime transcationDate)
        {
            Account cash = accountHandler.FindAccount("cash", AccountType.Assets);
            Account purchase = accountHandler.FindAccount("Purchases", AccountType.Expense);

            AccountDetail cashDetail = new AccountDetail();
            cashDetail.Cr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.AccountId = cash.Id;
            cashDetail.IsActive = true;

            AccountDetail purchaseDetail = new AccountDetail();
            purchaseDetail.Dr = amount;
            purchaseDetail.CreatedOn = DateTime.UtcNow;
            purchaseDetail.AccountId = purchase.Id;
            purchaseDetail.IsActive = true;

            Transaction trnscation = new Transaction();
            trnscation.AccountDetails.Add(cashDetail);
            trnscation.AccountDetails.Add(purchaseDetail);

            trnscation.ReceiptId = invoiceId;
            trnscation.Narration = "Goods Purchaesed";
            trnscation.CreatedOn = DateTime.UtcNow;
            trnscation.Date = transcationDate;
            trnscation.IsActive = true;
            trnscation.Type = (int)TransactionType.Sales;
            trnscation.IsActive = true;

            trnsHandler.Add(trnscation);
        }
    }
}
