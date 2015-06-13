using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CqrsSeed.Web.Api.Infrastructure;
using CqrsSeed.Web.Api.SignalrHandlers;

namespace CqrsSeed.Web.Api.Installer
{
    public class WebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
             container.Register(
                 Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>(),

                 Component.For(typeof(HubContextResolver)).LifestyleTransient(),

                 Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient(),

                 Component.For<IHttpControllerActivator>().UsingFactoryMethod(k => new WindsorHttpControllerActivator(k)),

                 // SignalR Event Handlers
                 Classes
                    .FromThisAssembly()
                    .Where(Component.IsInSameNamespaceAs<InventoryEventHandlers>())
                    .WithServiceAllInterfaces()
                    .LifestyleTransient());
        }
    }
}
