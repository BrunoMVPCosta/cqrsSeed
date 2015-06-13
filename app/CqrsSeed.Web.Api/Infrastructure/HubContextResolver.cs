using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CqrsSeed.Web.Api.Infrastructure
{
    public class HubContextResolver
    {
        public IHubContext Resolve<THub>() where THub : IHub
        {
            return GlobalHost.ConnectionManager.GetHubContext<THub>();
        }
    }
}