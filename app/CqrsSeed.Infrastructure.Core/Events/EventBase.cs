using System;
using Newtonsoft.Json;

namespace CqrsSeed.Infrastructure.Core.Events
{
	public class EventBase : IMessage
	{
        [JsonIgnore]
        public virtual int Version { get; set; }

        [JsonIgnore]
        public virtual Guid AggregateId { get; set; }
	}
}

