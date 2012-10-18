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
        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            // See if the session is active with a real user
            ApplicationState appState = httpContext.Session[Trips4.DRCOGApp.SessionIdentifier] as ApplicationState;
            if ((appState == null) ||
                (appState.CurrentUser == null) ||
                (appState.CurrentUser.profile == null) ||
                (appState.CurrentUser.profile.PersonGUID == Guid.Empty))
            {
                return false;
            }

            // the usual authorization
            return base.AuthorizeCore(httpContext);
        }
    }
}



