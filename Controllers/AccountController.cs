using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProje.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcProje.Controllers
{
    public class AccountController : Controller
    {
        NORTHWNDEntities db = new NORTHWNDEntities();
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Roles.RoleExists("user"))
                    {
                        Roles.CreateRole("user");
                        WebSecurity.CreateUserAndAccount(user.UserName, user.Password, new { user.Address, user.Email, user.FirsName, user.Surname });
                        return RedirectToAction("Index","Home");
                    }
                }
                catch (MembershipCreateUserException ex)
                {
                    return RedirectToAction("Error", new { errorMessage = ex.Message });
                }
            }
            return View(user);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserDetail user)
        {
            if (WebSecurity.Login(user.UserName,user.Password))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Error", "Kullanıcı Adı ya da Parola Yanlış!");
            }
        }
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult UserDetail()
        {
            var currentUser = db.UserDetails.Find(WebSecurity.CurrentUserId);
            return View(currentUser);
        }
        public ActionResult Error(string errorMessage)
        {
            ViewBag.Message = errorMessage;
            return View();
        }
    }
}
 