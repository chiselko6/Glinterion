using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace Glinterion.Common
{
    public class RolesAuthorizeAttribute : AuthorizeAttribute
    {
        private IGenericRepository<User> usersDb;
        private readonly string[] allowedroles;

        public RolesAuthorizeAttribute(params string[] Roles)
        {
            this.allowedroles = Roles;
            usersDb = DependencyResolver.Current.GetService<IGenericRepository<User>>();

        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = usersDb.Get(u => u.Login == httpContext.User.Identity.Name);
            if (allowedroles.Contains(user.Role.Name))
            {
                return true;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
