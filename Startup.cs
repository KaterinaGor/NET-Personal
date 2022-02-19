using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Firma.Startup))]
namespace Firma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
