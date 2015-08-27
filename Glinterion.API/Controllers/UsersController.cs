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
using h = System.Web.Http;

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

        [h.Authorize]
        [RolesAuthorize("admin", "moderator")]
        [h.Route("~/api/users")]
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
        [h.Authorize]
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

        [h.Authorize]
        [h.Route("~/api/users/user")]
        [h.HttpGet]
        public User GetUserInfo()
        {
            var userName = User.Identity.Name;
            var user = usersDb.Get(u => u.Login == userName);
            return user;
        }

    private bool UserExists(int id)
        {
            return usersDb.GetAll().Count(e => e.UserId == id) > 0;
        }
    }

}