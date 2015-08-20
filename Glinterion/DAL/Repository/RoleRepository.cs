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
    public class RoleRepository : IRoleRepository
    {
        private GlinterionContext db;

        public RoleRepository(GlinterionContext context)
        {
            db = context;
        }

        public IQueryable<Role> GetRoles()
        {
            return db.Roles;
        }

        public IQueryable<Role> GetRoles(Expression<Func<Role, bool>> predicate)
        {
            return (predicate == null ? GetRoles() : db.Roles.Where(predicate));
        }

        public IQueryable<User> GetUsersByRole(Role role)
        {
            return db.Entry(role).Entity.Users.AsQueryable();
        }

        public Role GetRole(int id)
        {
            return db.Roles.Find(id);
        }

        public Role GetRole(Expression<Func<Role, bool>> predicate)
        {
            return (predicate == null ? null : db.Roles.FirstOrDefault(predicate));
        }

        public Role GetRole(string roleName)
        {
            try
            {
                return db.Roles.First(role => role.Name == roleName);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void AddRole(Role role)
        {
            db.Roles.Add(role);
        }

        public void DeleteRole(int id)
        {
            var user = GetRole(id);
            db.Roles.Remove(user);
        }

        public void UpdateRole(Role role)
        {
            db.Entry(role).State = EntityState.Modified;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}