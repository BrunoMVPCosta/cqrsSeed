using System;
using System.Collections.Generic;
using CqrsSeed.Infrastructure.Core.Events;
using CqrsSeed.Infrastructure.Core.Utils;

namespace CqrsSeed.Infrastructure.Core.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<EventBase> _changes = new List<EventBase>();
       
        public AggregateRoot()
        {
            this.Version = -1;
        }

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        public IEnumerable<EventBase> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<EventBase> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(EventBase @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(EventBase @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            this.Version++;
            @event.Version = this.Version;
            @event.AggregateId = this.Id;
            if(isNew) _changes.Add(@event);
        }
    }
}