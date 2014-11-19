using App.Api.Filters;
using App.Api.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace App.Api
{
    public sealed class Configuration : HttpConfiguration
    {
        public Configuration()
        {
            //Services.Add(typeof(IExceptionHandler), null);

            //Formatters.Insert(0, new JsonNetMediaTypeFormatter());
            Filters.Add(new CultureFilterAttribute());
            this.MapHttpAttributeRoutes();
        }
    }
}