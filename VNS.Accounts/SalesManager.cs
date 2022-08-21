using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;


namespace VNS.Accounts
{
    public class SalesManager
    {
        AccountHandler accountHandler;
        TransactionHandler transactionHandler;
        public SalesManager(VNSAccountsDBEntities context)
        {
            accountHandler = new AccountHandler(context);
            transactionHandler = new TransactionHandler(context);
        }

        public void CreateSale(long invoiceId, decimal amount, DateTime transcationDate, long? clientId = null)
        {
            Account cash = accountHandler.FindAccount("cash", AccountType.Assets);
            //Account stock = accountHandler.FindAccount("stock", AccountType.Assets);
            Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue);
            //Account client = accountHandler.FindAccount(clientId);

            AccountDetail cashDetail = new AccountDetail();
            cashDetail.Dr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.AccountId = cash.Id;
            cashDetail.IsActive = true;

            //AccountDetail stockDetail = new AccountDetail();
            //stockDetail.Cr = amount;
            //stockDetail.CreatedOn = DateTime.UtcNow;
            //stockDetail.AccountId = stock.Id;
            //stockDetail.IsActive = true;

            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Cr = amount;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = sales.Id;
            salesDetail.IsActive = true;

            //AccountDetail clientDetail = new AccountDetail();
            //clientDetail.Cr = amount;
            //clientDetail.CreatedOn = DateTime.UtcNow;
            //clientDetail.AccountId = client.Id;
            //clientDetail.IsActive = true;

            Transaction trnscation = new Transaction();
            trnscation.AccountDetails.Add(cashDetail);
           // trnscation.AccountDetails.Add(stockDetail);
            trnscation.AccountDetails.Add(salesDetail);

            trnscation.ReceiptId = invoiceId;
            trnscation.Narration = "Items Sales";
            trnscation.CreatedOn = DateTime.UtcNow;
            trnscation.Date = transcationDate;
            trnscation.IsActive = true;
            trnscation.Type = (int)TransactionType.Sales;
            trnscation.IsActive = true;

            transactionHandler.Add(trnscation);
        }

        public void ReturnSale(long invoiceId, decimal amount, DateTime transcationDate, long? clientId = null)
        {
            Account cash = accountHandler.FindAccount("cash", AccountType.Assets);
            //Account stock = accountHandler.FindAccount("stock", AccountType.Assets);
            Account sales = accountHandler.FindAccount("Sales", AccountType.Revenue);
            //Account client = accountHandler.FindAccount(clientId);

            AccountDetail cashDetail = new AccountDetail();
            cashDetail.Cr = amount;
            cashDetail.CreatedOn = DateTime.UtcNow;
            cashDetail.AccountId = cash.Id;
            cashDetail.IsActive = true;

            //AccountDetail stockDetail = new AccountDetail();
            //stockDetail.Dr = amount;
            //stockDetail.CreatedOn = DateTime.UtcNow;
            //stockDetail.AccountId = stock.Id;
            //stockDetail.IsActive = true;

            AccountDetail salesDetail = new AccountDetail();
            salesDetail.Dr = amount;
            salesDetail.CreatedOn = DateTime.UtcNow;
            salesDetail.AccountId = sales.Id;
            salesDetail.IsActive = true;

            //AccountDetail clientDetail = new AccountDetail();
            //clientDetail.Cr = amount;
            //clientDetail.CreatedOn = DateTime.UtcNow;
            //clientDetail.AccountId = client.Id;
            //clientDetail.IsActive = true;

            Transaction trnscation = new Transaction();
            trnscation.AccountDetails.Add(cashDetail);
            //trnscation.AccountDetails.Add(stockDetail);
            trnscation.AccountDetails.Add(salesDetail);

            trnscation.ReceiptId = invoiceId;
            trnscation.Narration = "Sales returned.";
            trnscation.CreatedOn = DateTime.UtcNow;
            trnscation.Date = transcationDate;
            trnscation.IsActive = true;
            trnscation.Type = (int)TransactionType.Sales;
            trnscation.IsActive = true;

            transactionHandler.Add(trnscation);
        }
    }
}
