using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace App.Api.Filters
{
    public sealed class CultureFilterAttribute : ActionFilterAttribute
    {
        private bool HasCultureCookie(HttpRequestMessage request)
        {
            string cultureName;

            return HasCultureCookie(request, out cultureName);
        }

        private bool HasCultureCookie(HttpRequestMessage request, out string cultureName)
        {
            IEnumerable<CookieHeaderValue> cultureCookie = request.Headers.GetCookies("app.culture");

            if (cultureCookie.Count() > 0)
            {
                cultureName = cultureCookie.First().Cookies.First().Value;

                return true;
            }
            else
            {
                cultureName = null;

                return false;
            }
        }

        public void EvalCulture(HttpActionContext actionContext)
        {
            string isoCultureName;

            if (HasCultureCookie(actionContext.Request, out isoCultureName))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(isoCultureName);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(ConfigurationManager.AppSettings["defaultCulture"]);
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            EvalCulture(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!HasCultureCookie(actionExecutedContext.Request))
            {
                CookieHeaderValue cookie = new CookieHeaderValue("app.culture", Thread.CurrentThread.CurrentCulture.Name);
                cookie.Path = "/";

                actionExecutedContext.Response.Headers.AddCookies(new[] { cookie });
            }
        }
    }
}