using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(twright_FinacialPortal.Startup))]
namespace twright_FinacialPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
