using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(modelTest.Startup))]
namespace modelTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
