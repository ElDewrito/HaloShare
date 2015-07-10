using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HaloShare.Startup))]
namespace HaloShare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
