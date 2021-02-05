using System.Web.Mvc;
using System.Web.Routing;

namespace IdentitySample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{group}/{id}/{productName}",
                defaults: new { controller = "Home", action = "Index", group = "1", id = UrlParameter.Optional, productName= UrlParameter.Optional }
            );
        }
    }
}