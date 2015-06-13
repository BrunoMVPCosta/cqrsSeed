using CqrsSeed.BC.Inventory.Events;
using CqrsSeed.BC.Inventory.ReadModel;
using CqrsSeed.Infrastructure.Core.Bus;
using CqrsSeed.Infrastructure.Core.Events;
using CqrsSeed.Web.Api.Hubs;
using CqrsSeed.Web.Api.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace CqrsSeed.Web.Api.SignalrHandlers
{
    public class InventoryEventHandlers : IHandles<InventoryItemCreated>, IHandles<InventoryItemRenamed>,
        IHandles<InventoryItemDeactivated>, IHandles<ItemsCheckedInToInventory>, IHandles<ItemsRemovedFromInventory>
    {
        private readonly HubContextResolver _hubResolver;
        private readonly IReadModelFacade _readModel;

        private IHubContext Hub
        {
            get { return _hubResolver.Resolve<InventoryHub>(); }
        }

        public InventoryEventHandlers(HubContextResolver hubResolver, IReadModelFacade readModel)
        {
            _hubResolver = hubResolver;
            _readModel = readModel;
        }

        public void Handle(InventoryItemCreated message)
        {
            var details = _readModel.GetInventoryItemDetails(message.AggregateId);
            Hub.Clients.All.inventoryItemCreated(details);

            var list = _readModel.GetInventoryItems();
            Hub.Clients.All.inventoryItemsListUpdated(list);
        }

        public void Handle(InventoryItemRenamed message)
        {
            this.InventoryItemChanged(message);

            var list = _readModel.GetInventoryItems();
            Hub.Clients.All.inventoryItemsListUpdated(list);
        }

        public void Handle(InventoryItemDeactivated message)
        {
            Hub.Clients.All.inventoryItemRemoved(message.AggregateId);

            var list = _readModel.GetInventoryItems();
            Hub.Clients.All.inventoryItemsListUpdated(list);
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            this.InventoryItemChanged(message);
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            this.InventoryItemChanged(message);
        }

        private void InventoryItemChanged(EventBase ev)
        {
            var details = _readModel.GetInventoryItemDetails(ev.AggregateId);
            Hub.Clients.All.inventoryItemChanged(details);
        }

        public void Dispose()
        {
        }
    }
}