using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.BLL;
using VNS.Accounts.DAL;

namespace VNS.Accounts
{
    public static class AccountsStartUpManager
    {
        public static void CreateSetupAccounts()
        {
            AccountHandler handler = new AccountHandler(new VNSAccountsDBEntities());

            Account cash = new Account
            {
                Name = "Cash",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDefualt = true,
                Type = (int)AccountType.Assets
            };
            if (!handler.IsAccountExits(cash))
            {
                handler.Add(cash);
            }

            //Account stock = new Account
            //{
            //    Name = "Stock",
            //    CreatedOn = DateTime.UtcNow,
            //    IsActive = true,
            //    IsDefualt = true,
            //    Type = (int)AccountType.Assets
            //};
            //if (!handler.IsAccountExits(stock))
            //{
            //    handler.Add(stock);
            //}

            Account Sales = new Account
            {
                Name = "Sales",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDefualt = true,
                Type = (int)AccountType.Revenue
            };
            if (!handler.IsAccountExits(Sales))
            {
                handler.Add(Sales);
            }

            Account purchase = new Account
            {
                Name = "Purchases",
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                IsDefualt = true,
                Type = (int)AccountType.Expense
            };
            if (!handler.IsAccountExits(purchase))
            {
                handler.Add(purchase);
            }

            //Account Capital = new Account
            //{
            //    Name = "Capital",
            //    CreatedOn = DateTime.UtcNow,
            //    IsActive = true,
            //    IsDefualt = true,
            //    Type = (int)AccountType.Revenue
            //};
            //if (!handler.IsAccountExits(Capital))
            //{
            //    handler.Add(Capital);
            //}



        }
    }
}
