using CqrsSeed.BC.Inventory.Events;
using CqrsSeed.BC.Inventory.ReadModel.Dtos;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;
using CqrsSeed.Infrastructure.Core.Bus;
using MongoDB.Driver.Builders;

namespace CqrsSeed.BC.Inventory.ReadModelHandlers
{
    public class InventoryItemDetailView : IHandles<InventoryItemCreated>, IHandles<InventoryItemDeactivated>, IHandles<InventoryItemRenamed>, IHandles<ItemsRemovedFromInventory>, IHandles<ItemsCheckedInToInventory>
    {
        private readonly MongoRepository _repository;

        public InventoryItemDetailView(MongoRepository repository)
        {
            _repository = repository;
        }

        public void Handle(InventoryItemCreated message)
        {
            _repository.Insert(new InventoryItemDetailsDto(message.AggregateId, message.Name, 0,0));
        }

        public void Handle(InventoryItemRenamed message)
        {
            var a = _repository.FindOne<InventoryItemDetailsDto>(message.AggregateId);
            _repository.Update(message.AggregateId, Update<InventoryItemDetailsDto>.Set(d => d.Name, message.NewName).Set(d => d.Version, message.Version));
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            var a = _repository.FindOne<InventoryItemDetailsDto>(message.AggregateId);
            _repository.Update(message.AggregateId, Update<InventoryItemDetailsDto>.Inc(d => d.CurrentCount, -message.Count).Set(d => d.Version, message.Version));
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            var a = _repository.FindOne<InventoryItemDetailsDto>(message.AggregateId);
            _repository.Update(message.AggregateId, Update<InventoryItemDetailsDto>.Inc(d => d.CurrentCount, message.Count).Set(d => d.Version, message.Version));
        }

        public void Handle(InventoryItemDeactivated message)
        {
            var a = _repository.FindOne<InventoryItemDetailsDto>(message.AggregateId);
            _repository.Remove<InventoryItemDetailsDto>(message.AggregateId);
        }

        public void Dispose()
        {
        }
    }
}