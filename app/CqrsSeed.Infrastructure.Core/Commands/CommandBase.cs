using System;

namespace CqrsSeed.Infrastructure.Core.Commands
{
    public class CommandBase : IMessage
    {
        protected CommandBase(Guid inventoryItemId, int originalVersion)
        {
            this.AggregateId = inventoryItemId;
            this.Version = originalVersion;
        }

        public int Version { get; private set; }

        public Guid AggregateId { get; private set; }
	}
}
