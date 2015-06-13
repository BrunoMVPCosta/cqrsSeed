using System;
using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.BC.Inventory.Commands
{
    public class CreateInventoryItem : CommandBase {
        public readonly string Name;

        public CreateInventoryItem(Guid inventoryItemId, string name)
            : base(inventoryItemId, 0) 
        {
            Name = name;
        }
    }
}