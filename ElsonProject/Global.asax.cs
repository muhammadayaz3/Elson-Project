using ElsonProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ElsonProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Dictionary<string, DateTime> ExpiredAuthCookies = new Dictionary<string, DateTime>();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null && !CheckExpiredAuth(authCookie.Value))
                {
                    var id = HttpContext.Current.User.Identity as FormsIdentity;

                    if (id != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        var user = serializer.Deserialize<UsersModel>(id.Ticket.UserData);

                        HttpContext.Current.User = new MyPrincipal(id, user);
                    }
                }
            }
        }

        private bool CheckExpiredAuth(string value)
        {
            bool result = ExpiredAuthCookies.ContainsKey(value);

            foreach (var item in ExpiredAuthCookies.Where(x => (DateTime.Now - x.Value).Minutes > 90))
                ExpiredAuthCookies.Remove(item.Key);

            return result;
        }

    }
}
