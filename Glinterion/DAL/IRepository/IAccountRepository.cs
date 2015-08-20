using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL.IRepository
{
    public interface IAccountRepository
    {
        IQueryable<Account> GetAccounts();
        IQueryable<Account> GetAccounts(Expression<Func<Account, bool>> predicate);
        IQueryable<User> GetUsersByAccount(Account account);
        Account GetAccount(Expression<Func<Account, bool>> predicate);
        Account GetAccount(int id);
        void AddAccount(Account account);
        void DeleteAccount(int id);
        void UpdateAccount(Account account);
        void Save();
    }
}