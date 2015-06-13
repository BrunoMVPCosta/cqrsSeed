using System.Configuration;
using System.Net;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CqrsSeed.Infrastructure.Core.EventStore;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CqrsSeed.Infrastructure.EventStore.Installer
{
    public class EventStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var eventStoreAddress = ConfigurationManager.AppSettings["EventStoreAddress"].Split(':');
            var eventStoreCredentials = ConfigurationManager.AppSettings["EventStoreCredentials"].Split(':');
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse(eventStoreAddress[0]), int.Parse(eventStoreAddress[1])));
            var credentials = new UserCredentials(eventStoreCredentials[0], eventStoreCredentials[1]);
            connection.Connect();

            container.Register(

                Component.For<UserCredentials>().Instance(credentials),

                Component.For<IEventStoreConnection>().Instance(connection),

                Component.For(typeof (IRepository<>)).ImplementedBy(typeof (GetEventStoreRepository<>)).LifestyleTransient(),

                Component.For<EventStorePublisher>().LifestyleSingleton());

            container.Resolve<EventStorePublisher>().Start();
        }
    }
}
