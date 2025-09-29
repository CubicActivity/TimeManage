using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeManage.Startup))]
namespace TimeManage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
