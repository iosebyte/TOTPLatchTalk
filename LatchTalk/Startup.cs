using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LatchTalk.Startup))]
namespace LatchTalk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
