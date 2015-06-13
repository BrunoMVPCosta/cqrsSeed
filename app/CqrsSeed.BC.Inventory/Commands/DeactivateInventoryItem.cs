using System;
using CqrsSeed.Infrastructure.Core.Commands;

namespace CqrsSeed.BC.Inventory.Commands
{
    public class DeactivateInventoryItem : CommandBase {

        public DeactivateInventoryItem(Guid inventoryItemId, int originalVersion) : base (inventoryItemId, originalVersion)
        {
        }
    }
}