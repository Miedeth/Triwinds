using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Triwinds.Startup))]
namespace Triwinds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
