using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace CqrsSeed.Web.Api.Infrastructure
{
    public class IoCContainer : ScopeContainer, IDependencyResolver
    {
        public IoCContainer(IWindsorContainer container)
            : base(container)
        {
        }

        public IDependencyScope BeginScope()
        {
            var child = new WindsorContainer();
            Container.AddChildContainer(child);
            return new ScopeContainer(child);
        }
    }
}