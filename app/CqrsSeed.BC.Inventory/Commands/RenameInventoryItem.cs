using System;
using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.BC.Inventory.Commands
{
    public class RenameInventoryItem : CommandBase {
        public readonly string NewName;
        
        public RenameInventoryItem(Guid inventoryItemId, string newName, int originalVersion)
            : base(inventoryItemId, originalVersion) 
        {
            NewName = newName;
        }
    }
}