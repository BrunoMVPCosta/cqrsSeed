using System;

namespace CqrsSeed.Infrastructure.Core
{
	public interface IMessage
	{
        int Version { get; }

        Guid AggregateId { get; }
	}
}

