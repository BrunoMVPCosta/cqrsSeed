using CqrsSeed.BC.Inventory.Commands;
using CqrsSeed.BC.Inventory.Domain;
using CqrsSeed.Infrastructure.Core.Bus;
using CqrsSeed.Infrastructure.Core.EventStore;

namespace CqrsSeed.BC.Inventory.Handlers
{
    public class InventoryCommandHandlers : IHandles<CreateInventoryItem>, IHandles<DeactivateInventoryItem>, IHandles<RemoveItemsFromInventory>, IHandles<CheckInItemsToInventory>, IHandles<RenameInventoryItem>
    {
        private readonly IRepository<InventoryItem> _repository;
        public InventoryCommandHandlers(IRepository<InventoryItem> repository)
        {
            _repository = repository;
        }
        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.AggregateId, message.Name);
            _repository.Save(item);
        }
        public void Handle(DeactivateInventoryItem message)
        {
            var item = _repository.GetById(message.AggregateId, message.Version);
            item.Deactivate();
            _repository.Save(item);
        }
        public void Handle(RemoveItemsFromInventory message)
        {
            var item = _repository.GetById(message.AggregateId, message.Version);
            item.Remove(message.Count);
            _repository.Save(item);
        }
        public void Handle(CheckInItemsToInventory message)
        {
            var item = _repository.GetById(message.AggregateId, message.Version);
            item.CheckIn(message.Count);
            _repository.Save(item);
        }
        public void Handle(RenameInventoryItem message)
        {
            var item = _repository.GetById(message.AggregateId, message.Version);
            item.ChangeName(message.NewName);
            _repository.Save(item);
        }

        public void Dispose()
        {
            
        }
    }

}
