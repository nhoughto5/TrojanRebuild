using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrojanWebRebuild.Startup))]
namespace TrojanWebRebuild
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
