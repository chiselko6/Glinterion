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
    public class UserRepository : IUserRepository
    {
        private GlinterionContext db;

        public UserRepository(GlinterionContext context)
        {
            db = context;
        }

        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        public IQueryable<User> GetUsers(Expression<Func<User, bool>> predicate)
        {
            return (predicate == null ? GetUsers() : db.Users.Where(predicate));
        }

        public User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public User GetUser(Expression<Func<User, bool>> predicate)
        {
            return (predicate == null ? null : db.Users.FirstOrDefault(predicate));
        }
        
        public void AddUser(User user)
        {
            db.Users.Add(user);
        }

        public void DeleteUser(int userId)
        {
            var user = GetUser(u => u.UserId == userId);
            if (user == null)
            {
                return;
            }
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