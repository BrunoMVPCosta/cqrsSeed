using Castle.Windsor;
using CqrsSeed.BC.Inventory.Installer;
using CqrsSeed.Infrastructure.Core.Installer;
using CqrsSeed.Infrastructure.EventStore.Installer;
using CqrsSeed.Web.Api.Installer;
using Microsoft.Practices.ServiceLocation;

namespace CqrsSeed.Web.Api
{
    public static class ContainerConfig
    {
        public static IWindsorContainer Register()
        {
            var container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new CommonServiceLocator.WindsorAdapter.WindsorServiceLocator(container));

            container.Install(new InfrastructureInstaller());
            container.Install(new EventStoreInstaller());
            container.Install(new InventoryInstaller());
            container.Install(new WebApiInstaller());

            return container;
        }
    }
}
