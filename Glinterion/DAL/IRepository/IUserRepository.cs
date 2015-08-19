using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glinterion.Models;

namespace Glinterion.DAL.IRepository
{
    public interface IUserRepository : IDisposable
    {
        IQueryable<User> GetUsers();
        IQueryable<User> GetUserByRole(string role);
        int GetUserID(string userLogin);
        void AddUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user);
        void Save();
    }
}
