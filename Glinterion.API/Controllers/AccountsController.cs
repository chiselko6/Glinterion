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
using Glinterion.DAL;
using Glinterion.DAL.IRepository;
using Glinterion.Models;

namespace Glinterion.Controllers
{
    public class AccountsController : ApiController
    {
        private IGenericRepository<Account> accountsDb;
        private IGenericRepository<AccountSerial> accountsSerialsDb;
        private IGenericRepository<User> usersDb; 

        public AccountsController()
        {
            var uof = DependencyResolver.Current.GetService<IUnitOfWork>();
            accountsDb = uof.Repository<Account>();
            accountsSerialsDb = uof.Repository<AccountSerial>();
            usersDb = uof.Repository<User>();
        }

        // GET: api/Accounts
        [System.Web.Http.Authorize(Roles = "admin")]
        public IQueryable<Account> GetAccounts()
        {
            return accountsDb.GetAll().AsQueryable();
        }

        // GET: api/Accounts/5
        [ResponseType(typeof(Account))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult GetAccount(int id)
        {
            Account account = accountsDb.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/Accounts/5
        [ResponseType(typeof(void))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult PutAccount(int id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.AccountId)
            {
                return BadRequest();
            }

            accountsDb.Update(account);

            try
            {
                accountsDb.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Accounts
        [ResponseType(typeof(Account))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            accountsDb.Add(account);
            accountsDb.Save();

            return CreatedAtRoute("DefaultApi", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [ResponseType(typeof(Account))]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IHttpActionResult DeleteAccount(int id)
        {
            Account account = accountsDb.GetById(id);
            if (account == null)
            {
                return NotFound();
            }

            accountsDb.Delete(account);
            accountsDb.Save();

            return Ok(account);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("api/accounts/check")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Check(string serial)
        {
            var userName = User.Identity.Name;
            var user = usersDb.Get(u => u.Login == userName);
            // TODO:
            var accountSerial = accountsSerialsDb.Get(ser => ser.Serial == serial);
            if (accountSerial == null) return new HttpResponseMessage(HttpStatusCode.Forbidden);
            //var account = accountSerial.Account;
            user.AccountId = accountSerial.Account.AccountId;
            usersDb.Update(user);
            usersDb.Save();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private bool AccountExists(int id)
        {
            return accountsDb.GetAll().Count(e => e.AccountId == id) > 0;
        }
    }
}