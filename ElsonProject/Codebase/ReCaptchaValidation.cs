using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ElsonProject.Codebase
{
    public class ReCaptchaValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var recaptchaResponse = context.HttpContext.Request.Form["g-recaptcha-response"];

            if (!VerifyReCaptcha(recaptchaResponse))
            {
                var routeData = context.RouteData.Values;
                var actionName = routeData["action"].ToString();
                var controllerName = routeData["controller"].ToString();

                //context.Controller.ViewBag.Message = "Invalid Request";
                //context.Controller.ViewData["ReCaptchaError"]="Invalid Request";
                context.Controller.TempData["ReCaptchaError"] = "Invalid Request";

                context.Result = new ContentResult
                {
                    Content = "Invalid Request",
                    ContentType = "text/plain"
                };
                context.Result = new HttpStatusCodeResult(400, "Bad Request: Invalid Request.");
                return;
            }
            base.OnActionExecuting(context);
        }
        public bool VerifyReCaptcha(string token)
        {
            try
            {
                var secretKey = ConfigurationManager.AppSettings["RecaptchaSecretKey"].ToString();
                var recaptchaurl = ConfigurationManager.AppSettings["RecaptchaURL"].ToString();
                //Log.Information("{Account,VerifyReCaptcha}", "URL[" + recaptchaurl + "], SecretKey[" + secretKey + "]");

                var client = new HttpClient();
                var response = client.PostAsync(string.Format(recaptchaurl, secretKey, token), null).Result;
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(jsonResult);
                //Log.Information("{Account,VerifyReCaptcha}", "ReCaptcha is Valid");
                return result.success == "true";

            }
            catch (Exception ex)
            {
                //Log.Error("{Account,VerifyReCaptcha}", "ReCaptcha is not Valid");
                //Log.Error("{Account,VerifyReCaptcha}", "Exception[" + ex + "]");
                return false;
            }

        }
    }
}