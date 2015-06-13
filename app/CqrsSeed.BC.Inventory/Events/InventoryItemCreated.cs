using System;
using CqrsSeed.Infrastructure.Core.Events;

namespace CqrsSeed.BC.Inventory.Events
{
    public class InventoryItemCreated : EventBase {
        public readonly Guid Id;
        public readonly string Name;
        public InventoryItemCreated(Guid id, string name) {
            Id = id;
            Name = name;
        }
    }
}