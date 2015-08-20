using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Glinterion.Models;

namespace Glinterion.DAL.IRepository
{
    public interface IUserRepository : IDisposable
    {
        IQueryable<User> GetUsers();
        IQueryable<User> GetUsers(Expression<Func<User, bool>> predicate);
        User GetUser(int id);
        User GetUser(Expression<Func<User, bool>> predicate);
        void AddUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user);
        void Save();
    }
}
