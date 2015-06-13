using CqrsSeed.BC.Inventory.Events;
using CqrsSeed.BC.Inventory.ReadModel.Dtos;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;
using CqrsSeed.Infrastructure.Core.Bus;
using MongoDB.Driver.Builders;

namespace CqrsSeed.BC.Inventory.ReadModelHandlers
{
    public class InventoryListView : IHandles<InventoryItemCreated>, IHandles<InventoryItemRenamed>, IHandles<InventoryItemDeactivated>
    {
        private readonly MongoRepository _repository;

        public InventoryListView(MongoRepository repository)
        {
            _repository = repository;
        }

        public void Handle(InventoryItemCreated message)
        {
            _repository.Insert(new InventoryItemListDto(message.AggregateId, message.Name));
        }

        public void Handle(InventoryItemRenamed message)
        {
            _repository.Update(message.AggregateId, Update<InventoryItemListDto>.Set(d => d.Name, message.NewName));
        }

        public void Handle(InventoryItemDeactivated message)
        {
            _repository.Remove<InventoryItemListDto>(message.AggregateId);
        }

        public void Dispose()
        {
        }
    }
}