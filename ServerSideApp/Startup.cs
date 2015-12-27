using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServerSideApp.Startup))]
namespace ServerSideApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
