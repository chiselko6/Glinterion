using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Glinterion.Providers;
using Glinterion.ViewModels;

namespace Glinterion.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customMembership = (CustomMembershipProvider)Membership.Provider;
            var membershipUser = customMembership.CreateUser(model);
            if (membershipUser == null)
            {
                ModelState.AddModelError(string.Empty, "This login already exists!");
            }
            FormsAuthentication.SetAuthCookie(model.Email, false);
            return Redirect("/App_JS/index.html#/profile/" + model.Login);
        }
    }
}