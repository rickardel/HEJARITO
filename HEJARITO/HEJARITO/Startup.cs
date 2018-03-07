using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HEJARITO.Startup))]
namespace HEJARITO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
