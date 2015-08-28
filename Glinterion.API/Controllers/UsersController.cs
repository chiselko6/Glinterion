using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Security;
using Glinterion.Common;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using Route = System.Web.Http.RouteAttribute;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using HttpGet = System.Web.Http.HttpGetAttribute;

namespace Glinterion.Controllers
{
    public class UsersController : ApiController
    {
        private IGenericRepository<User> usersDb;
        private IGenericRepository<Account> accountsDb; 

        public UsersController()
        {
            var uof = DependencyResolver.Current.GetService<IUnitOfWork>();
            usersDb = uof.Repository<User>();
            accountsDb = uof.Repository<Account>();
        }

        [Authorize]
        [RolesAuthorize("admin", "moderator")]
        [Route("~/api/users")]
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return usersDb.GetAll().AsQueryable();
        }

        // GET: api/Users/5
        //[ResponseType(typeof (User))]
        //[ApplicationAuthorize("admin", "moderator")]
        //public IHttpActionResult GetUser(int id)
        //{
        //    User user = usersDb.GetById(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        // DELETE: api/Users/5
        [ResponseType(typeof (User))]
        [Authorize]
        [RolesAuthorize("admin", "moderator")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = usersDb.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            usersDb.Delete(user);
            usersDb.Save();

            return Ok(user);
        }

        //private User Clean(User user)
        //{
        //    var dumpSerials = user.Account.Serials;
        //    user.Account.Serials = null;
        //    var dumpAccountUsers = user.Account.Users;
        //    user.Account.Users = null;
        //    var dumpRoleUsers = user.Role.Users;
        //    user.Role.Users = null;
        //    var dumpUserPassword = user.Password;
        //    user.Password = null;
        //    return user;
        //}

        [Authorize]
        [Route("~/api/users/user")]
        [HttpGet]
        public User GetUserInfo()
        {
            var userName = User.Identity.Name;
            //return Clean(usersDb.Get(u => u.Login == userName));
            return usersDb.Get(u => u.Login == userName);
        }

    private bool UserExists(int id)
        {
            return usersDb.GetAll().Count(e => e.UserId == id) > 0;
        }
    }

}