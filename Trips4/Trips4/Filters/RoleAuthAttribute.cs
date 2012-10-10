//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/17/2009 1:52:14 PM
// Description:
//
//======================================================
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using DRCOG.Web.Utilities.ApplicationState;
using System.Web.Security;
using DRCOG.TIP.Services;
using DRCOG.Data;

namespace DRCOG.Web.Filters
{

    /// <summary>
    /// Custom Authorize filter for the DRCOG application that utilizes
    /// the DRCOG Account entity that's stored in session for logged in
    /// users.
    /// </summary>
    public class RoleAuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string _roles;
        private string[] _rolesSplit = new string[0];

        /// <summary>
        /// Roles who can access this method
        /// </summary>
        public string Roles
        {
            get
            {
                return _roles ?? String.Empty;
            }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        private ProfileServiceExtender ProfileService { get; set; }

        public RoleAuthAttribute()
        {
            ProfileService = new ProfileServiceExtender();
        }


        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.

        //TODO: Refactor
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            //See if the session is active
            ApplicationState appState = this.GetSession(httpContext);
            if (appState == null)
            {                
                return false;
            }
            else
            {
                //Get the user
                Person user = appState.CurrentUser;
                
                if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// Gets the session if it exists.
        /// </summary>
        /// <returns></returns>
        private ApplicationState GetSession(HttpContextBase httpContext)
        {
            var sessionObject = httpContext.Session[DRCOGApp.SessionIdentifier];
            if (sessionObject != null)
                return sessionObject as ApplicationState;
            else
            {
                httpContext.Session.Add(DRCOGApp.SessionIdentifier, new ApplicationState());
                return httpContext.Session[DRCOGApp.SessionIdentifier] as ApplicationState;
            }
        }

       


        /// <summary>
        /// Check the role authorization
        /// </summary>
        /// <param name="filterContext"></param>
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //See if the session is active
            HttpContextBase ctx = filterContext.HttpContext;
            if (ctx.Session != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;

                if (ctx.Session.IsNewSession && !ticket.IsPersistent)
                {
                    
                    // from:  http://www.tyronedavisjr.com/index.php/2008/11/23/detecting-session-timeouts-using-a-aspnet-mvc-action-filter/
                    // If it says it is a new session, but an existing cookie exists, then it must  
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary {
                              { "message", "Session Timed Out" },
                              { "controller", "Login" },
                              { "action", "index" },
                              { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                        });
                    }
                }
                else
                {
                    //an existing session...
                    ApplicationState appState = this.GetSession(filterContext.HttpContext);
                    Person user;

                    if (ticket.IsPersistent)
                    {
                        user = new Person(HttpContext.Current.User.Identity.Name);
                        user.Load();
                        user.profile = ProfileService.GetUserProfile(user.profile.UserName, Common.Services.MemberShipServiceSupport.MembershipProviderType.DRCOG);

                        appState.CurrentUser = user;
                    }
                    else
                    {
                        user = appState.CurrentUser;
                        if (user == null || String.IsNullOrEmpty(user.profile.UserName))
                        {
                            user = new Person(HttpContext.Current.User.Identity.Name);
                            user.Load();
                            user.profile = ProfileService.GetUserProfile(user.profile.UserName, Common.Services.MemberShipServiceSupport.MembershipProviderType.DRCOG);

                            appState.CurrentUser = user;
                        }
                    }

                    user.profile.Roles["TripsRoleProvider"] = ProfileService.GetRolesForUser(user.profile.UserName, Common.Services.MemberShipServiceSupport.RoleProviderType.TRIPS);

                    //user.LoadRoles(HttpContext.Current.User);


                    if (user.SponsorsProject())
                    {
                        user.AddRole("Sponsor");
                    }

                    //user.AddRole("Sponsor");

                    //Check if user is in the authorized roles
                    if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
                    {
                        //User not in role - unauthorized access attempt (or we messed up and exposed a link to the wrong class of user
                        UnauthorizedViewModel model = new UnauthorizedViewModel();
                        
                        //model.CurrentUser = user;
                        //model.Message = String.Join(",", user.profile.Roles) + " The resource you attempted to access is restricted. This access attempt has been logged.";
                        model.Message = Roles + " The resource you attempted to access is restricted. This access attempt has been logged.";
                        ViewDataDictionary viewData = new ViewDataDictionary(model);
                        filterContext.Result = new ViewResult { ViewName = "~/Views/Error/Unauthorized.aspx", ViewData = viewData };
                        //If the controller is not null, use it's logger!
                        if (filterContext.Controller != null)
                        {
                            DRCOG.Web.Controllers.ControllerBase ctl = filterContext.Controller as DRCOG.Web.Controllers.ControllerBase;
                            //ctl.Logger.Log.Warn("Unauthorized attempt to access " + filterContext.HttpContext.Request.RawUrl + " by " + user.Login);
                        }
                    }
                    else
                    {
                        //User is in the role... let'er rip
                        // ** IMPORTANT ** (Note from the Microsoft AuthorizeAttribute source code
                        // Since we're performing authorization at the action level, the authorization code runs
                        // after the output caching module. In the worst case this could allow an authorized user
                        // to cause the page to be cached, then an unauthorized user would later be served the
                        // cached page. We work around this by telling proxies not to cache the sensitive page,
                        // then we hook our custom authorization code into the caching mechanism so that we have
                        // the final say on whether a page should be served from the cache.
                        HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                        cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                        cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
                    }

                }
            }
            else
            {
                //Null session
                filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary {
                              { "message", "You must login before accessing that page." },
                              { "controller", "Login" },
                              { "action", "index" },
                              { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                        });                
            }
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

    }
}



