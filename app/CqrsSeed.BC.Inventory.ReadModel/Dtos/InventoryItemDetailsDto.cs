using System;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;

namespace CqrsSeed.BC.Inventory.ReadModel.Dtos
{
    public class InventoryItemDetailsDto : MongoEntity
    {
        public string Name;
        public int CurrentCount;
        public int Version;

        public InventoryItemDetailsDto(Guid id, string name, int currentCount, int version)
        {
			Id = id;
			Name = name;
            CurrentCount = currentCount;
            Version = version;
        }
    }
}
