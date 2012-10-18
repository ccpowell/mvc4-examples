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

    public class CheckSessionModule : IHttpModule
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(DRCOGApp_AuthenticateRequest);
            context.PostAuthenticateRequest += new EventHandler(DRCOGApp_PostAuthenticateRequest);
            context.PostAuthorizeRequest += new EventHandler(DRCOGApp_PostAuthorizeRequest);
            context.AcquireRequestState += new EventHandler(DRCOGApp_AcquireRequestState);
            Logger.Debug("CheckSessionModule Initialized");
        }

        public void DRCOGApp_AcquireRequestState(object sender, EventArgs e)
        {
            Logger.Debug("DRCOGApp_AcquireRequestState");
        }

        public void DRCOGApp_PostAuthorizeRequest(object sender, EventArgs e)
        {
            Logger.Debug("DRCOGApp_PostAuthorizeRequest");
        }

        public void DRCOGApp_PostAuthenticateRequest(object sender, EventArgs e)
        {
            Logger.Debug("DRCOGApp_PostAuthenticateRequest");
        }

        public void DRCOGApp_AuthenticateRequest(object sender, EventArgs e)
        {
            Logger.Debug("DRCOGApp_AuthenticateRequest");
        }
    }

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class DRCOGApp : System.Web.HttpApplication
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public const string SessionIdentifier = "TripsSessionIdentifier";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Initialise();

        }
    }
}