using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CqrsSeed.BC.Inventory.Handlers;
using CqrsSeed.BC.Inventory.ReadModel;
using CqrsSeed.BC.Inventory.ReadModel.Mongo;
using CqrsSeed.BC.Inventory.ReadModelHandlers;
using MongoDB.Driver;

namespace CqrsSeed.BC.Inventory.Installer
{
    public class InventoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var mongoUrl = ConfigurationManager.AppSettings["BC.Inventory.MongoUrl"]; // "mongodb://127.0.0.1:27017"
            var mongoDbName = ConfigurationManager.AppSettings["BC.Inventory.MongoDbName"]; // "CqrsSeed.BC.Inventory.ReadModel

            var mongoClient = new MongoClient(new MongoUrl(mongoUrl));
            var mongoServer = mongoClient.GetServer();

            container.Register(
                // Command Handlers
                Classes
                    .FromAssemblyContaining<InventoryCommandHandlers>()
                    .Pick()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient(),

                // Read Model
                Component.For<MongoDatabase>().UsingFactoryMethod(k => mongoServer.GetDatabase(mongoDbName)),
                Component.For<MongoRepository>().LifestyleTransient(),
                Component.For<IReadModelFacade>().ImplementedBy<ReadModelFacade>().LifestyleTransient(),

                // Read Model Handlers
                Classes
                    .FromAssemblyContaining<InventoryItemDetailView>()
                    .Where(Component.IsInSameNamespaceAs<InventoryItemDetailView>())
                    .WithServiceAllInterfaces()
                    .LifestyleTransient());
        }
    }
}
