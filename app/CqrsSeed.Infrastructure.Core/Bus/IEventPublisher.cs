using CqrsSeed.Infrastructure.Core.Events;

namespace CqrsSeed.Infrastructure.Core.Bus
{
    public interface IEventPublisher
    {
        void Publish(EventBase @event);
    }
}