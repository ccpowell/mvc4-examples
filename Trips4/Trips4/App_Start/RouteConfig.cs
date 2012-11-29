using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Trips4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("favicon.ico");

            routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                "AccountRoutes",                                              // Route name
                "Account/{action}/{id}",                           // URL with parameters
                new { controller = "Account", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "LoginRoutes",                                              // Route name
                "Login/{action}",                           // URL with parameters
                new { controller = "Login", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
                "TipYear",                                              // Route name
                "{controller}/{year}/{action}/{id}",                           // URL with parameters
                new { controller = "TIP", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "Home",                                              // Route name
                "{controller}/{action}",                           // URL with parameters
                new { controller = "Home", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{id}/{action}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }
    }
}