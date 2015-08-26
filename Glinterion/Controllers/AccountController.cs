using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                return View();
            }
            FormsAuthentication.SetAuthCookie(model.Login, false);
            return RedirectToAction("App", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var validUser = Membership.ValidateUser(model.Login, model.Password);
            if (!validUser)
            {
                ModelState.AddModelError(string.Empty, "User doesn't exist");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.Login, true);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("App", "Home");
        }

        //[Authorize]
        //public new ActionResult Profile()
        //{
        //    return Redirect("/App_JS/index.html#/profile/" + User.Identity.Name);
        //}

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}