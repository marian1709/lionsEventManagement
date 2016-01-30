using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lionsEventManagement.Startup))]
namespace lionsEventManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
