using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Switchboard.Startup))]
namespace Switchboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
