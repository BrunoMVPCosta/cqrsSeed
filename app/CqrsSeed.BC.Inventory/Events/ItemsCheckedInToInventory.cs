using CqrsSeed.Infrastructure.Core.Events;

namespace CqrsSeed.BC.Inventory.Events
{
    public class ItemsCheckedInToInventory : EventBase
    {
        public readonly int Count;
 
        public ItemsCheckedInToInventory(int count) {
            Count = count;
        }
    }
}