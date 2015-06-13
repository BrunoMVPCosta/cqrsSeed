using System;
using System.Collections.Generic;
using CqrsSeed.Infrastructure.Core.Domain;

namespace CqrsSeed.Infrastructure.Core.EventStore
{
    public interface IRepository<T> where T : AggregateRoot
    {
        void Save(T aggregate, Action<IDictionary<string, object>> updateHeaders = null);
        T GetById(Guid id);
        T GetById(Guid id, int version);
    }
}