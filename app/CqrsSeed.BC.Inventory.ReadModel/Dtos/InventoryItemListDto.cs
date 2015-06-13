using System;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;

namespace CqrsSeed.BC.Inventory.ReadModel.Dtos
{
    public class InventoryItemListDto : MongoEntity
    {
        public string Name;

        public InventoryItemListDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}