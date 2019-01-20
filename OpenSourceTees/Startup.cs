using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpenSourceTees.Startup))]
namespace OpenSourceTees
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
