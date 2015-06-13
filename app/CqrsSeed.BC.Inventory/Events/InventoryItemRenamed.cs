using CqrsSeed.Infrastructure.Core.Events;

namespace CqrsSeed.BC.Inventory.Events
{
    public class InventoryItemRenamed : EventBase
    {
        public readonly string NewName;
 
        public InventoryItemRenamed(string newName)
        {
            NewName = newName;
        }
    }
}