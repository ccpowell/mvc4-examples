using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Trips4
{
    public class DRCOGApp : System.Web.HttpApplication
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public const string SessionIdentifier = "TripsSessionIdentifier";

        protected void Application_Start()
        {
            Logger.Info("TRIPS starting");

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialise();
        }

        protected void Application_End()
        {
            Logger.Info("TRIPS ending");
        }
    }
}