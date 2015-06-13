using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CqrsSeed.Infrastructure.Core.Bus;

namespace CqrsSeed.Infrastructure.Core.Installer
{
    public class InfrastructureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                // BUS
                Classes
                    .FromAssemblyContaining<InMemoryBus>()
                    .Pick()
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton());
        }
    }
}
