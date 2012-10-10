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
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Common.Services.MemberShipServiceSupport.SSO.Domain;
using DRCOG.Common.Web.MvcSupport.Attributes;
using DRCOG.Domain;
//using DTS.Web.MVC;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Security;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.ViewModels;
using DRCOG.TIP.Services;
using DRCOG.Web.Filters;
using DRCOG.Web.Utilities.ApplicationState;

namespace DRCOG.Web.Controllers
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
    [HandleError]
    //[RemoteRequireHttps]
    public class LoginController : ControllerBase
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        

        /// <summary>
        /// IAuthenticator instance that abstracts the actual
        /// authentication logic. Allows for testing.
        /// </summary>
        public IAccountRepository AccountRepository { get; set; }
        //public IMembershipService MembershipService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref=this.ControllerName/> class.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        public LoginController(IAccountRepository accountRepository, IUserRepositoryExtension userRepository)
            : base("LoginController", userRepository)
        {
            AccountRepository = accountRepository;
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

                FormsAuthentication.SignOut();
                Session.Abandon();

                // Clear invalid login text
                var viewModel = new LoginViewModel 
                {
                    Message = Request["message"] ?? TempData["message"] as String ?? String.Empty,
                    ReturnUrl = Request["ReturnUrl"] ?? String.Empty
                };
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
                if (ModelState.IsValid)
                {
                    this.LoadSession();

                    if (model.UserName.Contains("@"))
                    {
                        model.UserName = Membership.GetUserNameByEmail(model.UserName);
                    }

                    Person person = new Person(model.UserName);

                    ValidateUserResultType result;
                    // First try to authenicate through service
                    if (!(result = SSOFederationHelper.FederationObject.ValidateUser(model.UserName, model.Password)).Equals(ValidateUserResultType.Error))
                    {
                        return base.SetAuthCookie(model, result, returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");

                        string exceptionMessage;
                        bool isApproved = UserService.GetUserApproval(model.UserName);
                        if ((bool)isApproved) exceptionMessage = "The user name or password provided is incorrect.";
                        else exceptionMessage = "Your account has not been activated. Please click on the link in your verification email or use the link above to resend the verification email to this email address.";
                        //ModelState.AddModelError("", exceptionMessage);
                        viewModel.Message = exceptionMessage;
                        viewModel.RefreshAssemblyVersion();
                    }
                }
                else
                {
                    //check to see if user is not approved
                    string exceptionMessage = "The user name or password provided is incorrect.";

                    //pass back the error
                    viewModel.Message = exceptionMessage;
                    viewModel.RefreshAssemblyVersion();
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

        

        /// <summary>
        /// Logs the user out of the application and re-directs to the 
        /// login page
        /// </summary>
        /// <returns></returns>
        //[RoleAuth]
        [HttpGet]
        public ActionResult Logout()
        {
            var ReturnUrl = Request["ReturnUrl"] ?? String.Empty;
            try
            {
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
            catch (Exception ex)
            {
                ErrorViewModel model = new ErrorViewModel("An unexpected error has occurred while attempting to log you out of the system.", "This error has been logged.", ex, this.ControllerName, "Logout");
                return View("CustomError", model);
            }
        }
    }
}