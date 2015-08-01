using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GesTenis.Startup))]
namespace GesTenis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
