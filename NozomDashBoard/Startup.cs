using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Web.Routing;

[assembly: OwinStartup(typeof(NozomDashBoard.Startup))]
namespace NozomDashBoard
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
