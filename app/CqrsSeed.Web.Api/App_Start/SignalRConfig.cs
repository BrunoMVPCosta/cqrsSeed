using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CqrsSeed.Web.Api.SignalRConfig))]

namespace CqrsSeed.Web.Api
{
    public class SignalRConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
