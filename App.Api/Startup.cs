using Owin;

namespace App.Api
{
    public sealed class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(new Configuration());
        }
    }
}
