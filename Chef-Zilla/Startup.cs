using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chef_Zilla.Startup))]
namespace Chef_Zilla
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
