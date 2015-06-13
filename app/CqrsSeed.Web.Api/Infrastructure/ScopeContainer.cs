using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace CqrsSeed.Web.Api.Infrastructure
{
    public class ScopeContainer : IDependencyScope
    {
        protected IWindsorContainer Container;

        public ScopeContainer(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            return Container.Kernel.HasComponent(serviceType) ? Container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.Kernel.HasComponent(serviceType)
                ? Container.ResolveAll(serviceType).Cast<object>()
                : new object[0];
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}