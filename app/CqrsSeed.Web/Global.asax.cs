using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CqrsSeed.BC.Inventory.Installer;
using CqrsSeed.Infrastructure.Core.Installer;
using CqrsSeed.Infrastructure.EventStore.Installer;
using Microsoft.Practices.ServiceLocation;

namespace CqrsSeed.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InitContainer();
            RegisterRoutes(RouteTable.Routes);
        }

        private void InitContainer()
        {
            var container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new CommonServiceLocator.WindsorAdapter.WindsorServiceLocator(container));

            container.Install(new InfrastructureInstaller());
            container.Install(new EventStoreInstaller());
            container.Install(new InventoryInstaller());

            container.Register(Classes.FromThisAssembly().BasedOn<Controller>().LifestyleTransient());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
        }
    }
}