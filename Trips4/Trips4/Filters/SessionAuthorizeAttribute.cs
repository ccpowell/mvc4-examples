using System;
using System.Web;
using System.Web.Mvc;
using Trips4.Utilities.ApplicationState;

namespace Trips4.Filters
{

    /// <summary>
    /// Custom Authorize filter for the DRCOG application that checks
    /// the DRCOG Account entity that's stored in session for logged in
    /// users. Authorization fails if the session has timed out, i.e. 
    /// if the DRCOG data is not present in the session. The rest of 
    /// the authorization algorithm is implemented by the base class.
    /// </summary>
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            // See if the session is active with a real user
            var appState = httpContext.Session[Trips4.DRCOGApp.SessionIdentifier] as ApplicationState;
            bool noUser = ((appState == null) ||
                (appState.CurrentUser == null) ||
                (appState.CurrentUser.profile == null) ||
                (appState.CurrentUser.profile.PersonGUID == Guid.Empty));
            if (noUser)
            {
                // not authorized if session timed out.
                // But if the identity is not authenticated then the user didn't even log in.
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    Logger.Debug("User session timed out.");
                }
                else
                {
                    Logger.Debug("User session has no user.");
                }
                return false;
            }

            // the usual authorization
            return base.AuthorizeCore(httpContext);
        }
    }
}



