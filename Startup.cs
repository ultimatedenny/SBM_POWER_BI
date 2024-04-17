using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SBM_POWER_BI.Startup))]
namespace SBM_POWER_BI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
