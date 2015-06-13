using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CqrsSeed.Infrastructure.Core.Domain;
using CqrsSeed.Infrastructure.Core.EventStore;
using CqrsSeed.Infrastructure.Core.EventStore.Exceptions;
using CqrsSeed.Infrastructure.Core.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CqrsSeed.Infrastructure.EventStore
{
    public class GetEventStoreRepository<TAggregate> : IRepository<TAggregate>, IDisposable where TAggregate : AggregateRoot
    {

        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;
        private const int SnapshotStep = 2;

        private readonly Func<Type, Guid, string> _aggregateIdToStreamName = (t, g) => string.Format("{0}-{1}", char.ToLower(t.Name[0]) + t.Name.Substring(1), g.ToString("N"));
        private readonly Func<Type, Guid, string> _aggregateIdToSnapshotStreamName = (t, g) => string.Format("{0}-snapshot-{1}", char.ToLower(t.Name[0]) + t.Name.Substring(1), g.ToString("N"));
        private readonly IEventStoreConnection _eventStoreConnection;
        
        private static readonly JsonSerializerSettings SerializerSettings;

        static GetEventStoreRepository()
        {
            var dcr = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            dcr.DefaultMembersSearchFlags |= System.Reflection.BindingFlags.NonPublic;

            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None, ContractResolver = dcr};
        }

        public GetEventStoreRepository(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;

        }

        public TAggregate GetById(Guid id)
        {
            return GetById(id, int.MaxValue);
        }

        public TAggregate GetById(Guid id, int version)
        {
            if (version < 0)
                throw new InvalidOperationException("Cannot get version < 0");

            var streamName = _aggregateIdToStreamName(typeof(TAggregate), id);
            var aggregate = LoadFromSnapshot(id, version) ?? ConstructAggregate();

            var sliceStart = aggregate.Version + 1;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + ReadPageSize <= version
                                    ? ReadPageSize
                                    : version - sliceStart + 1;

                if(sliceCount <= 0)
                {
                    break;
                }

                currentSlice = _eventStoreConnection.ReadStreamEventsForward(streamName, sliceStart, sliceCount, false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof(TAggregate));

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof(TAggregate));

                sliceStart = currentSlice.NextEventNumber;

                aggregate.LoadsFromHistory(currentSlice.Events.Select(evnt => DeserializeObject<EventBase>(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data)));
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            if (aggregate.Version != version && version < Int32.MaxValue)
                throw new AggregateVersionException(id, typeof(TAggregate), aggregate.Version, version);

            return aggregate;
        }

        public void Save(TAggregate aggregate, Action<IDictionary<string, object>> updateHeaders)
        {
            var commitId = Guid.NewGuid();
            var commitHeaders = new Dictionary<string, object>
            {
                {CommitIdHeader, commitId},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };

            if (updateHeaders != null)
            {
                updateHeaders(commitHeaders);
            }

            var streamName = _aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            var newEvents = aggregate.GetUncommittedChanges().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var eventsToSave = newEvents.Select(e => ToEventData(aggregate.Id, e, commitHeaders)).ToList();


            if (eventsToSave.Count < WritePageSize)
            {
                _eventStoreConnection.AppendToStream(streamName, originalVersion, eventsToSave);
            }
            else
            {
                var transaction = _eventStoreConnection.StartTransaction(streamName, originalVersion);

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    transaction.Write(pageEvents);
                    position += WritePageSize;
                }

                transaction.Commit();
            }

            aggregate.MarkChangesAsCommitted();

            if (Enumerable.Range(aggregate.Version + 1, newEvents.Count).Any(v => v % SnapshotStep == 0))
            {
                this.SaveSnapshot(aggregate);
            }
            
        }

        #region Helpers

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        private static TAggregate ConstructAggregate()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static T DeserializeObject<T>(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return (T)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName), SerializerSettings);
        }

        #endregion Helpers

        #region Snapshotting

        private TAggregate LoadFromSnapshot(Guid id, int version)
        {
            var streamName = _aggregateIdToSnapshotStreamName(typeof(TAggregate), id);

            var sliceStart = StreamPosition.End;
            var sliceCount = ReadPageSize;
            StreamEventsSlice currentSlice;
            do
            {

                currentSlice = _eventStoreConnection.ReadStreamEventsBackward(streamName, sliceStart, sliceCount, false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    return null;

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof(TAggregate));

                sliceStart = currentSlice.NextEventNumber;

                foreach (var resolvedEvent in currentSlice.Events)
                {
                    var aggregate = DeserializeObject<TAggregate>(resolvedEvent.OriginalEvent.Metadata, resolvedEvent.OriginalEvent.Data);

                    if (aggregate.Version <= version)
                    {
                        return aggregate;
                    }
                }

            } while (!currentSlice.IsEndOfStream);

            return null;
        }
        
        private void SaveSnapshot(TAggregate aggregate)
        {
            var commitId = Guid.NewGuid();
            var commitHeaders = new Dictionary<string, object>
            {
                {CommitIdHeader, commitId},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };
            
            var streamName = _aggregateIdToSnapshotStreamName(aggregate.GetType(), aggregate.Id);

            _eventStoreConnection.AppendToStream(streamName, ExpectedVersion.Any, ToEventData(aggregate.Id, aggregate, commitHeaders));
        }

        #endregion Snapshotting

        public void Dispose()
        {
        }
    }
}
