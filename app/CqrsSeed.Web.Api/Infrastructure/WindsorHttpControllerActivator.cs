using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel;

namespace CqrsSeed.Web.Api.Infrastructure
{
    public class WindsorHttpControllerActivator : IHttpControllerActivator
    {
        private readonly IKernel _container;

        public WindsorHttpControllerActivator(IKernel container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController)_container.Resolve(controllerType);
        }
    }
}