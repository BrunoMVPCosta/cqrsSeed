using CqrsSeed.Infrastructure.Core.Events;

namespace CqrsSeed.BC.Inventory.Events
{
    public class ItemsRemovedFromInventory : EventBase
    {
        public readonly int Count;
 
        public ItemsRemovedFromInventory(int count) {
            Count = count;
        }
    }
}