using System;
using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.BC.Inventory.Commands
{
    public class RemoveItemsFromInventory : CommandBase {
        public readonly int Count;

        public RemoveItemsFromInventory(Guid inventoryItemId, int count, int originalVersion)
            : base(inventoryItemId, originalVersion) 
        {
            Count = count;
        }
    }
}