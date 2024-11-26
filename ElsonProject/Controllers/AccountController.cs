using ElsonProject.Codebase;
using ElsonProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ElsonProject.Controllers
{
    public class AccountController : Controller
    {

        AccountDBHandler account = new AccountDBHandler();
        UsersModel lg = new UsersModel();
        string _role = "";
        // GET: Account
        public ActionResult Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception)
            { throw; }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            string getotp = null;
            try
            {
                var user = account.UserLogin(collection["username"], collection["password"], collection["CompanyId"]);
                if (user.Id <= 0)
                {
                    ViewBag.Message = user.Reason;
                    return View();
                }
                else
                {
                    lg.Id = user.Id;
                    lg.UserId = user.Id;
                    lg.Username = user.Username;
                    lg.Password = Session["Password"].ToString();
                    lg.UserType =Convert.ToInt32(Session["UserType"].ToString());
                    lg.AcctType = (bool)Session["AccType"];

                    var serializer = new JavaScriptSerializer();
                    var userData = serializer.Serialize(lg);
                    var authTicket = new FormsAuthenticationTicket(
                    1,
                    lg.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(60),
                    false,
                    userData);

                    var hash = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    if (authTicket.IsPersistent) cookie.Expires = authTicket.Expiration;
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                    Response.Cookies.Add(cookie);
                    //Log.Information("{Account,VerifyOTP}", "Username[" + Session["Username"].ToString() + "], SubUserName[" + Session["SubUsername"].ToString() + "],Email[" + Session["UserEmail"].ToString() + "] ,OTP[" + getotp + "],Redirect:{Index,Home}");
                    Session["VerifyOTP"] = true;
                    return RedirectToAction("Index", "Home");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return View();
        }

        public void Logout()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                MvcApplication.ExpiredAuthCookies.Add(authCookie.Value, DateTime.Now);
            }

            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            HttpCookie cookie2 = new HttpCookie("__RequestVerificationToken", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie3 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie3.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie3);

            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);

            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            FormsAuthentication.RedirectToLoginPage();
        }
    }
}