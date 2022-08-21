using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerce.UI.Startup))]
namespace ECommerce.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
