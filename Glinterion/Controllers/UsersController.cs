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
using Glinterion.DAL;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

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

        [System.Web.Http.Authorize(Roles = "admin")]
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return usersDb.GetAll().AsQueryable();
        }

        // GET: api/Users/5
        [ResponseType(typeof (User))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult GetUser(int id)
        {
            User user = usersDb.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof (void))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            usersDb.Update(user);

            try
            {
                usersDb.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof (User))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usersDb.Add(user);
            usersDb.Save();

            return CreatedAtRoute("DefaultApi", new {id = user.UserId}, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof (User))]
        [System.Web.Http.Authorize(Roles = "admin")]
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

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("api/users/userAccount")]
        public Account GetUserAccount()
        {
            // TODO: 
            var userName = User.Identity.Name;
            var user = usersDb.Get(u => u.Login == userName);
            var account = user.Account;
            return account;
        }

    private bool UserExists(int id)
        {
            return usersDb.GetAll().Count(e => e.UserId == id) > 0;
        }
    }
}