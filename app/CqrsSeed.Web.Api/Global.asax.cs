using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CqrsSeed.Web.Api.Infrastructure;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;

namespace CqrsSeed.Web.Api
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            var container = ContainerConfig.Register();
            GlobalConfiguration.Configuration.DependencyResolver = new IoCContainer(container);
            
            SetCamelCaseForJsonSerializations();

        }

        private static void SetCamelCaseForJsonSerializations()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;
            
            formatters.Remove(formatters.XmlFormatter);
            var resolver = new SignalRContractResolver();
            formatters.JsonFormatter.SerializerSettings.ContractResolver = resolver;
            
            var serializer = new JsonSerializer { ContractResolver = resolver };
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer); 
        }
    }
}