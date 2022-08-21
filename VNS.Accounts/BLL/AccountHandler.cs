using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNS.Accounts.DAL;

namespace VNS.Accounts.BLL
{
    public class AccountHandler : IGenericRepository<Account>
    {
        VNSAccountsDBEntities context;
        public AccountHandler(VNSAccountsDBEntities context)
        {
            this.context = context;
        }
        public IEnumerable<Account> List
        {
            get
            {
                return context.Accounts.Where(x => x.IsActive);
            }
        }

        public void Add(Account entity)
        {
            context.Accounts.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Account entity)
        {
            entity.IsActive = false;
            Update(entity);
        }

        public Account FindById(long Id)
        {
            return context.Accounts.Find(Id);
        }

        public void Update(Account entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public bool IsAccountExits(Account entity)
        {
            var account = context.Accounts.SingleOrDefault(x => x.Name.ToLower().Equals(entity.Name.ToLower()) && x.Type == entity.Type);
            return account != null;
        }

        public Account FindAccount(string name, AccountType type)
        {
            return context.Accounts.FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()) && x.Type ==(int)type && x.IsActive);
        }
        public Account FindAccount(long clientId)
        {
            return context.Accounts.SingleOrDefault(x => x.ClientId == clientId && x.IsActive);
        }
    }
}
