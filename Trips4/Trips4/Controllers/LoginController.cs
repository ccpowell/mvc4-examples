#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 07/03/2009	Dave Bouwman    1. Initial Creation (DTS).
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
//using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
//using DRCOG.Common.Services.MemberShipServiceSupport;
//using DRCOG.Common.Services.MemberShipServiceSupport.SSO.Domain;
//using DRCOG.Common.Web.MvcSupport.Attributes;
using DRCOG.Domain;
//using DTS.Web.MVC;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Security;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.ViewModels;
using DRCOG.TIP.Services;
//using Trips4.Filters;
using Trips4.Utilities.ApplicationState;

namespace Trips4.Controllers
{
    /// <summary>
    /// Controller class for the login user interface.
    /// </summary>
    /// <remarks>
    /// The IIS 7.0 admin tool has an "SSL Settings" node that you can select for each site, directory or file that allows you to control whether that particular 
    /// resource (and by default its children) requires an SSL request in order to execute.  This is useful for pages like a login.aspx page, where you want to 
    /// guarantee that users can only enter their credentials when they are posting via an encrypted channel. If you configure the login.aspx page to require 
    /// SSL, IIS 7.0 will block browsers from accessing it unless they are doing so over SSL.
    /// </remarks>
    public class LoginController : ControllerBase
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref=this.ControllerName/> class.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        public LoginController(IAccountRepository accountRepository, ITripsUserRepository userRepository)
            : base("LoginController", userRepository)
        {
        }

        /// <summary>
        /// Base view showing the login form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                LoadSession();
                // But if the identity is not authenticated then the user didn't even log in.
                var appState = Session[Trips4.DRCOGApp.SessionIdentifier] as ApplicationState;
                bool noUser = (
                    (appState == null) ||
                    (appState.CurrentUser == null) ||
                    (appState.CurrentUser.profile == null) ||
                    (appState.CurrentUser.profile.PersonGUID == Guid.Empty));
                string timeoutMessage = string.Empty;
                if (noUser && User.Identity.IsAuthenticated)
                {
                    timeoutMessage = "Your session has timed out. Please log in again to proceed.";
                }

                FormsAuthentication.SignOut();
                Session.Abandon();

                // Clear invalid login text
                var viewModel = new LoginViewModel
                {
                    Message = Request["message"] ?? TempData["message"] as String ?? timeoutMessage,
                    ReturnUrl = Request["ReturnUrl"] ?? String.Empty
                };
                viewModel.RefreshAssemblyVersion();

                // N.B. Adding a header requires Integrated Pipeline mode, so either IIS or IIS Express is
                // required to run this (Cassini won't do it).
                // Adding this header allows the global ajax handler to detect the login page without
                // examining the HTML.
                Response.Headers.Add("LoginPage", "This is it.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("failure during login", ex);
                var viewModel = new ErrorViewModel("DRCOG TIP Application is unable to handle your request at this time.", "This error has been logged.", ex, this.ControllerName, "Index - Get");
                return View("CustomError", viewModel);
            }

        }

        /// <summary>
        /// Attempts to log the user into the system. If successful, user object stored in session.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(LoginViewModel viewModel, string returnUrl)
        {
            LogOnModel model = viewModel.LogOnModel;
            try
            {
                LoadSession();

                if (GuestUser(model))
                {
                    return base.SetAuthCookie(model, returnUrl);
                }

                viewModel.RefreshAssemblyVersion();
                if (ModelState.IsValid)
                {
                    this.LoadSession();

                    Person person = new Person(model.UserName);

                    // First try to authenicate through service
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        return base.SetAuthCookie(model, returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");

                        string exceptionMessage;
                        bool isApproved = UserService.GetUserApproval(model.UserName);
                        if (isApproved)
                            exceptionMessage = "The user name or password provided is incorrect.";
                        else
                            exceptionMessage = "Your account has not been activated. Please click on the link in your verification email or use the link above to resend the verification email to this email address.";
                        //ModelState.AddModelError("", exceptionMessage);
                        viewModel.Message = exceptionMessage;
                    }
                }
                else
                {
                    //pass back the error
                    viewModel.Message = "User name and password must be entered.";
                }

                return View(viewModel);
            }
            catch (SqlException sqlex)
            {
                //Send to error message on Login view
                viewModel.Message = "A database has occurred while attempting to log you into the system.";
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel(ex + "An unexpected error has occurred while attempting to log you into the system.", "This error has been logged.", ex, this.ControllerName, "Index - Post");
                return View("CustomError", error);
            }
        }

        private bool GuestUser(LogOnModel model)
        {
            if (model.LoginType != "guest")
            {
                return false;
            }
            model.UserName = "guest";
            model.Password = "!!Test123";

            // TODO: remove this
            //Membership.DeleteUser(model.UserName);

            // see if this is a valid user
            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                Logger.Debug("found guest");
                return true;
            }

            // if not, add the user and roles
            Logger.Debug("creating user " + model.UserName);
            var profile = new ShortProfile()
            {
                FirstName = "Anonymous",
                LastName = "Guest",
                Password = model.Password,
                RecoveryEmail = "trips.guest@drcog.org",
                UserName = model.UserName
            };
            _userRepository.CreateUserAndPerson(profile);
            return true;
        }



        /// <summary>
        /// Logs the user out of the application and re-directs to the 
        /// login page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            var ReturnUrl = Request["ReturnUrl"] ?? String.Empty;
            FormsAuthentication.SignOut();
            Session.Abandon();

            if (!String.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                // Take us back to the homepage after logout
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
