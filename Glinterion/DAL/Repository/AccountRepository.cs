using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private GlinterionContext db;

        public AccountRepository(GlinterionContext context)
        {
            db = context;
        }

        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        public IQueryable<Account> GetAccounts(Expression<Func<Account, bool>> predicate)
        {
            return (predicate == null ? GetAccounts() : db.Accounts.Where(predicate));
        }

        public IQueryable<User> GetUsersByAccount(Account account)
        {
            return db.Entry(account).Entity.Users.AsQueryable();
        }

        public Account GetAccount(Expression<Func<Account, bool>> predicate)
        {
            return (predicate == null ? null : db.Accounts.FirstOrDefault(predicate));
        }

        public Account GetAccount(int id)
        {
            return db.Accounts.Find(id);
        }
        
        public void AddAccount(Account account)
        {
            db.Accounts.Add(account);
        }

        public void DeleteAccount(int id)
        {
            var account = GetAccount(id);
            if (account == null)
            {
                return;
            }
            db.Accounts.Remove(account);
        }

        public void UpdateAccount(Account account)
        {
            db.Entry(account).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}