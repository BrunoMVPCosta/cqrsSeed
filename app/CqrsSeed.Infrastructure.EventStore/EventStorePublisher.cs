using System;
using System.Text;
using CqrsSeed.Infrastructure.Core.Bus;
using CqrsSeed.Infrastructure.Core.Events;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CqrsSeed.Infrastructure.EventStore
{
    public class EventStorePublisher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreConnection _eventStoreConnection;
        private UserCredentials _credentials;
        private const string EventClrTypeHeader = "EventClrTypeName";

        public EventStorePublisher(IEventStoreConnection eventStoreConnection, IEventPublisher eventPublisher, UserCredentials credentials)
        {
            _eventPublisher = eventPublisher;
            _eventStoreConnection = eventStoreConnection;
            _credentials = credentials;
        }

        public void Start()
        {
            _eventStoreConnection.SubscribeToAll(false, EventAppeared, SubscriptionDropped, _credentials);
        }

        private void SubscriptionDropped(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            //throw new NotImplementedException();
        }

        private void EventAppeared(EventStoreSubscription eventStoreSubscription, ResolvedEvent arg2)
        {
            if (arg2.Event.EventType.StartsWith("$"))
            {
                return;
            }

            var evnt = DeserializeEvent(arg2.OriginalEvent.Metadata, arg2.OriginalEvent.Data);
            if (evnt == null)
            {
                return;
            }

            evnt.AggregateId = arg2.OriginalEvent.EventId;
            evnt.Version = arg2.OriginalEvent.EventNumber;
            _eventPublisher.Publish(evnt);
        }

        private static EventBase DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName)) as EventBase;
        }
    }
}
