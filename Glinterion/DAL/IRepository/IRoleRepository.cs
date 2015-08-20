using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Glinterion.Models;

namespace Glinterion.DAL.IRepository
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetRoles();
        IQueryable<Role> GetRoles(Expression<Func<Role, bool>> predicate);
        IQueryable<User> GetUsersByRole(Role role);
        Role GetRole(int id);
        Role GetRole(Expression<Func<Role, bool>> predicate);
        void AddRole(Role role);
        void DeleteRole(int id);
        void UpdateRole(Role role);
        void Save();
    }
}