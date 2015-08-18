using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Glinterion.DAL.Contexts;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

namespace Glinterion.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private UsersContext db;

        public UserRepository(UsersContext context)
        {
            db = context;
        }

        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        public IQueryable<User> GetUserByRole(string role)
        {
            return db.Users.Where(user => user.Role == role);
        }

        public User GetUserById(int id)
        {
            return db.Users.Find(id);
        }

        public void AddUser(User user)
        {
            db.Users.Add(user);
        }

        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            db.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}