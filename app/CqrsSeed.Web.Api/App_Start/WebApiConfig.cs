using System.Web.Http;

namespace CqrsSeed.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "apiRoute",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "apiActionRoute",
                routeTemplate: "{controller}/{id}/{action}");
        }
    }
}
