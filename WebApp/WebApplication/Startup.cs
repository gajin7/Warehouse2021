using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using WebApplication;

[assembly: OwinStartup(typeof(Startup))]
namespace WebApplication
{
    // In this class we will Configure the OAuth Authorization Server.
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}