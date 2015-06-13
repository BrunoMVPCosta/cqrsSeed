using System;
using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.BC.Inventory.Commands
{
    public class CheckInItemsToInventory : CommandBase {
        public readonly int Count;

        public CheckInItemsToInventory(Guid inventoryItemId, int count, int originalVersion) : base (inventoryItemId, originalVersion) 
        {
            Count = count;
        }
    }
}