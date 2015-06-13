using System;
using System.Collections.Generic;
using System.Linq;
using CqrsSeed.Infrastructure.Core.Commands;
using CqrsSeed.Infrastructure.Core.Events;
using CqrsSeed.Infrastructure.Core.Utils;
using Microsoft.Practices.ServiceLocation;

namespace CqrsSeed.Infrastructure.Core.Bus
{
    public class InMemoryBus : ICommandSender, IEventPublisher
    {
        public void Send(CommandBase command)
        {
            var handlers = this.GetCommandActions(command.GetType()).ToList();
            if (handlers.Any())
            {
                if (handlers.Count() != 1) throw new InvalidOperationException("cannot send to more than one handler");
                handlers.First().AsDynamic().Handle(command);
                ((IDisposable)handlers.First()).Dispose();
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

        public void Publish(EventBase @event)
        {
            var handlers = this.GetCommandActions(@event.GetType()).ToList();
            if (!handlers.Any()) return;
            foreach(var handler in handlers)
            {
                var handler1 = handler;
                handler1.AsDynamic().Handle(@event);
                ((IDisposable)handler).Dispose();
            }
            
        }

        private IEnumerable<object> GetCommandActions(Type commandType)
        {
            var type = typeof (IHandles<>).MakeGenericType(commandType);
            return ServiceLocator.Current.GetAllInstances(type);
        }
    }
}
