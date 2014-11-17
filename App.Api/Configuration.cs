using App.Api.Filters;
using System.Web.Http;

namespace App.Api
{
    public sealed class Configuration : HttpConfiguration
    {
        public Configuration()
        {
            this.Filters.Add(new CultureFilterAttribute());
            this.MapHttpAttributeRoutes();
        }
    }
}