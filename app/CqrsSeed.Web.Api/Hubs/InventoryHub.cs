using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CqrsSeed.Web.Api.Hubs
{
    [HubName("inventory")]
    public class InventoryHub : Hub
    {
    }
}