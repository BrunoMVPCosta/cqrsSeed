using System;

namespace CqrsSeed.Infrastructure.Core.Bus
{
    public interface IHandles<T> : IDisposable where T: IMessage
    {
        void Handle(T message);
    }
}