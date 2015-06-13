using System;
using System.Collections.Generic;
using CqrsSeed.BC.Inventory.ReadModel.Dtos;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;

namespace CqrsSeed.BC.Inventory.ReadModel
{
    public class ReadModelFacade : IReadModelFacade
    {
        private readonly MongoRepository _repository;

        public ReadModelFacade(MongoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<InventoryItemListDto> GetInventoryItems()
        {
            return _repository.FindAll<InventoryItemListDto>();
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            return _repository.FindOne<InventoryItemDetailsDto>(id);
        }
    }
}