using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgriculturaWeb.Startup))]
namespace AgriculturaWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
