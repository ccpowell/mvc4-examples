﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DTS.Web.MVC;
//using DTS.Web.MVC.Extensions;
using DTS.Extensions;
//using Trips4.Filters;
using DRCOG.Domain.ViewModels;
using Trips4.Configuration;
using Trips4.Utilities.ApplicationState;
using System.Web.Security;
using System.Net.Configuration;
using System.Configuration;
using Trips4.Services;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services;
using DRCOG.Domain.Security;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Common.Services.MemberShipServiceSupport.Validation;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using DRCOG.Common.Web.MvcSupport.Attributes;
using DRCOG.Common.Services.MemberShipServiceSupport.SSO.Domain;
using DRCOG.Common.Services.MemberShipServiceSupport.Domain;
using DRCOG.Common.Util;

namespace Trips4.Controllers
{
    /// <summary>
    /// Controller that will manage the Account Administration aspects of the DRCOG application.
    /// Actual login validation is handled in the <see cref="LoginController"/>
    /// </summary>
    [HandleError]
    //[RemoteRequireHttps]
    public class AccountController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IEmailService _emailService;
        private IUserService _userService;

        /// <summary>
        /// IFormsAuthentication instance that abstracts the actual
        /// authentication logic. Allows for testing.
        /// </summary>
        //private IFormsAuthentication _formsAuthentication;
        public IFormsAuthenticationService FormsService { get; set; }

        public IMembershipService MembershipService { get; set; }

        /// <summary>
        /// Inject the account repository
        /// </summary>
        /// <param name="accountRepository"></param>
        public AccountController(IAccountRepository accountRepository, IEmailService emailService, IUserRepositoryExtension userRepository)
            : base("AccountController", userRepository)      
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
            _userService = new UserService(userRepository);
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
        }

        [HttpPost]
        public JsonResult Register(Profile profile)
        {
            Person person = new Person();
            person.profile = profile;

            ApplicationState appSession = this.GetSession();
            if (appSession == null)
            {   // this can happen if a user lets the session time out on the login page...
                appSession = GetNewSession();
            }

            bool result = false;
            string message = String.Empty;
            try
            {
                // Test user auto delete
                if (profile.RecoveryEmail == "dtucker1.trips@drcog.dev")
                {
                    var user = SSOFederationHelper.FederationObject.GetUserByEmail(profile.RecoveryEmail, false);
                    if (user.UserName != null)
                    {
                        SSOFederationHelper.FederationObject.DeleteUser(user.UserName);
                    }
                }

                profile.BusinessEmail = profile.RecoveryEmail;
                
                // Attempt to register the user
                var createStatus = SSOFederationHelper.FederationObject.CreateUserWithProfile(profile, true);

                if (createStatus.StatusEnum == MembershipCreateStatus.Success)
                {
                    SSOFederationHelper.FederationObject.AddUserToRole(profile.UserName, "Viewer", RoleProviderType.TRIPS);
                    person.profile = SSOFederationHelper.FederationObject.GetUserByName(profile.UserName, true);

                    SendVerificationMail(person.profile);
                    result = true;
                }
                else
                {
                    return Json(new
                    {
                        data = result
                        ,
                        message = AccountValidation.ErrorCodeToString(createStatus.StatusEnum)
                        ,
                        error = "true"
                    });
                }

                // If we got this far, something failed, redisplay form
                ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = "Changes could not be stored. An error has been logged."
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                data = result
                ,
                message = "Account successfully created."
                ,
                error = "false"
            });
        }

        private void SendVerificationMail(Profile profile)
        {
            //Guid guid = (Guid)user.ProviderUserKey;

            DRCOGConfig config = DRCOGConfig.GetConfig();
            string emailConfirmationPage = config.EmailConfirmationPage;

            string emailTo = profile.BusinessEmail;
            string emailSubject = "DRCOG (T.R.I.P.S.) - Account Validation Email";
            string emailBody = "";
            string emailLink = emailConfirmationPage + profile.PersonShortGuid;

            //Make body of email
            //if (user.) emailBody = "<p>Dear " + firstName + ",</p>";
            emailBody = emailBody + "<p>Thank you for creating a T.R.I.P.S account. To activate your account " +
                " please click <a href=\"" + emailLink + "\">here</a>.</p>";
            emailBody = emailBody + "<p>After you have clicked on the link above, you will be able to sign " +
                "into the T.R.I.P.S. at www3.drcog.org/trips/.</p>";

            IEmailService mail = new EmailService()
            {
                Body = emailBody,
                Subject = emailSubject,
                To = emailTo
            };
            try
            {
                mail.Send("no.reply@drcog.org", "DRCOG (TRIPS) Account Verification Service");
            }
            catch (Exception exc)
            {
                Exception exci = new Exception("Re/SendVerificationEmail; TO:" + profile.UserName, exc);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exci);
            }
        }

        public ActionResult Verify(string id)
        {
            ApplicationState appSession = this.GetSession();
            if (appSession == null)
            {   // this can happen if a user lets the session time out on the login page...
                appSession = (ApplicationState)this.GetNewSession();
            }

            var user = SSOFederationHelper.FederationObject.GetUserByID(ShortGuid.Decode(id), false);

            //Guid guid = new Guid(id);
            //MembershipUser user = Membership.GetUser(guid);
            if (user != null && user.IsApproved == false)
            {
                SSOFederationHelper.FederationObject.UpdateUserApproval(user.PersonGUID, true);
                
                Person person = new Person() { profile = user };
                appSession.CurrentUser = person;
                Session["isValidated"] = true;
                TempData["Message"] = "Thank you for verifying your Account";
                
                return base.SetAuthCookie(new LogOnModel() { UserName = user.UserName }, ValidateUserResultType.Membership, String.Empty);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Message"] = "Thank you, Your account has already been verified.";
            }

            return RedirectToAction("Index", "Login");
        }

        //[Authorize]
        public ActionResult ChangePassword(string id)
        {
            var user = SSOFederationHelper.FederationObject.GetUserByID(ShortGuid.Decode(id), false);
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(new ChangePasswordModel() { UserName = user.UserName });
        }

        //[Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (SSOFederationHelper.FederationObject.ChangePassword(model.UserName, model.OldPassword, model.NewPassword))
                {
                    if (!(SSOFederationHelper.FederationObject.ValidateUser(model.UserName, model.NewPassword)).Equals(ValidateUserResultType.Error))
                    {
                        this.LoadSession();
                        //FormsService.SignIn(model.UserName, false);
                        this.SetAuthCookie(new LogOnModel() { UserName = model.UserName });

                        //ApplicationState appSession = this.GetSession();
                        //State appState = null;
                        //if (appSession == null)
                        //{   // this can happen if a user lets the session time out on the login page...
                        //    appSession = (ApplicationState)this.GetNewSession();
                        //    appState = (DefaultApplicationState)appSession.State;
                        //}

                        var user = SSOFederationHelper.FederationObject.GetUserByName(model.UserName, false);
                        Person person = new Person() { profile = user };
                        CurrentSessionApplicationState.CurrentUser = person;
                        this.SaveSession(CurrentSessionApplicationState);

                        TempData["Message"] = "Thank you. Your password has been successfully changed";
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ResendVerificationMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResendVerificationMail(LogOnModel model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                //MembershipUser user = Membership.GetUser(model.UserName);
                var user = SSOFederationHelper.FederationObject.GetUserByEmail(model.UserName, false);
                if (user != null && !user.IsApproved)
                {
                    SendVerificationMail(user);
                    TempData["Message"] = "The verification email has been resent to you, please click on the link to activate your profile";
                }
                else
                {
                    TempData["Message"] = "The email account entered is not valid,does not exist, or has already been validated.";
                }
            }
            return RedirectToAction("LogOn");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgotPassword(PasswordRecoveryModel model)
        {
            ModelState.Remove("Answer");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = SSOFederationHelper.FederationObject.GetUserByEmail(model.Email, false);

                    if (user == null)
                    {
                        return Json(new
                        {
                            data = false
                            ,
                            message = "The account requested does not exist. Please check your entry."
                            ,
                            error = "true"
                        });
                    }

                    DRCOGConfig config = DRCOGConfig.GetConfig();
                    string recoverypage = config.PasswordRecoveryPage;
                    string changepasswordpage = config.ChangePasswordBaseUrl;

                    PasswordResetResult result;

                    if ((result = SSOFederationHelper.FederationObject.ResetPassword(model.Email)).PasswordResetResultType.Equals(PasswordResetResultType.Successful))
                    {

                        IEmailService mail = new EmailService()
                        {
                            Body = "<h1>Password Recovery was Requested!</h1>" +
                                "Your new password is now set to:<br/><br/><b>" + result.Password + "</b><br/><br />" +
                                "-------------------------------------------------------------------<br/>" +
                                "<u><b>What do I do now?</b></u><br/>" +
                                "1. Copy the above password<br/>" +
                                "2. Login with the password OR Follow this link to change your password: " +
                                "<a href=\"" + changepasswordpage + user.PersonShortGuid + "\">Change your Password</a>",
                            Subject = "DRCOG (T.R.I.P.S.) - Password Recovery email",
                            To = model.Email
                        };

                        mail.Send("no.reply@drcog.org", "DRCOG (TRIPS) Password Recovery Service");
                        return Json(new
                        {
                            data = result
                            ,
                            message = "Please check your email and click on the link to reset your password."
                            ,
                            error = "false"
                        });
                    }

                    return Json(new
                    {
                        message = "There was an error resetting your password."
                        ,
                        error = "true"
                        ,
                        exceptionMessage = "Ensure that you have entered your recovery email account properly."
                    });

                }
                catch (Exception exc)
                {
                    Exception exci = new Exception("ForgotPassword; TO:" + model.Email, exc);
                    Elmah.ErrorSignal.FromCurrentContext().Raise(exci);

                    return Json(new
                    {
                        message = "There was an error sending your password email."
                        ,
                        error = "true"
                        ,
                        exceptionMessage = exc.Message
                    });
                }

            }

            return Json(new
            {
                message = "Changes could not be stored. An error has been logged."
                ,
                error = "true"
                ,
                exceptionMessage = String.Empty
            });
        }

        //public ActionResult PasswordRecovery(string id)
        //{
        //    try
        //    {
        //        MembershipUser user = Membership.GetUser(new Guid(id));
        //        PasswordRecoveryModel model = new PasswordRecoveryModel();
        //        model.Question = user.PasswordQuestion;
        //        model.Email = user.UserName;
        //        RecoveryViewModel viewModel = new RecoveryViewModel() { PasswordRecoveryModel = model };
        //        return View(viewModel);
        //    }
        //    catch (Exception exc)
        //    {
        //        return RedirectToAction("ForgotPassword");
        //    }
        //}

        //[HttpPost]
        //public ActionResult PasswordRecovery(RecoveryViewModel viewModel)
        //{
        //    PasswordRecoveryModel model = viewModel.PasswordRecoveryModel;
        //    try
        //    {
        //        if (MembershipService.ResetPassword(model.Email, model.Password, model.Answer))
        //        {

        //            if (MembershipService.ValidateUser(model.Email, model.Password))
        //            {
        //                FormsService.SignIn(model.Email, false);
                        
        //                ApplicationState appSession = this.GetSession();
        //                State appState = null;
        //                if (appSession == null)
        //                {   // this can happen if a user lets the session time out on the login page...
        //                    appSession = (ApplicationState)this.GetNewSession();
        //                    appState = (DefaultApplicationState)appSession.State;
        //                }

        //                Person person = new Person(model.Email);
        //                person.Load();

        //                appSession.CurrentUser = person;

        //                TempData["Message"] = "Thank you. Your password has been successfully changed";
        //                return RedirectToAction("Index", "Home");
        //            }
        //            return RedirectToAction("LogOn");
        //        }
        //        return View(model);

        //    }
        //    catch (Exception exc)
        //    {
        //        return View(model);
        //    }
        //}

        //Functions:
        //Search for Logins/Contacts
        //Show details of a login
        //Update a login


        /// <summary>
        /// Get the search view
        /// </summary>
        /// <returns></returns>
        //[RoleAuth(Roles="Administrator")]
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            //Return the SearchView
            ApplicationState appSession = (ApplicationState)Session[DRCOGApp.SessionIdentifier];
            //if (appSession == null || appSession.CurrentUser == null)
            //{
            //    //Always use lower case for redirects - helps with unit testing
            //    return RedirectToAction("index", "login");                
            //}
            //else
            //{
                //return the SearchView
                var viewModel = new AccountViewModel(new AccountDetailModel(), new AccountSearchModel());
                //viewModel.CurrentUser = appSession.CurrentUser;
                //Since we do not have any search criteria, or a current user, we do not need to 
                //setup anything on the submodels
                //**Need to specify the view name if we want to validate the view by name in tests
                return View("search", viewModel);
            //}
        }

        /// <summary>
        /// Search for users in the system
        /// </summary>
        /// <param name="search_type"></param>
        /// <param name="search_FirstName"></param>
        /// <param name="search_LastName"></param>
        /// <param name="search_email"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator")]
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Search(string search_type, string search_FirstName, string search_LastName, string search_email, int page)        
        //{
        //    JsonServerResponse jsonResponse = new JsonServerResponse();
        //    try
        //    {
        //        //ApplicationState appSession = (ApplicationState)Session[DRCOGApp.SessionIdentifier];
        //        //if (appSession == null || appSession.CurrentUser == null)
        //        //{
        //        //    return Json(new JsonServerResponse("Invalid session.", null), this.JsonResultContentType);
        //        //}

        //        //Get the list of customers for the criteria
        //        IList<Account> accounts = _accountRepository.SearchForAccounts(search_type, search_FirstName, search_LastName, search_email);

        //        if (accounts == null || accounts.Count == 0)
        //        {
        //            return Json(new JsonServerResponse("No acounts matched the search criteria.", null), this.JsonResultContentType);
        //        }

        //        //sort them
        //        accounts = accounts.OrderBy<Account, string>(x => x.LastName).ToList<Account>();

        //        //Paging
        //        int itemsPerPage = 10;
        //        int totalPages = (int)Math.Ceiling(double.Parse(accounts.Count.ToString()) / itemsPerPage);

        //        if (page < 1 || page > totalPages)
        //        {
        //            return Json(new JsonServerResponse("Invalid page of data (page " + page.ToString() + ") requested.", null), this.JsonResultContentType);
        //        }

        //        //use the DTS.Extensions.GetPage extension method to get the page
        //        IList<Account> returnList = new List<Account>();
        //        returnList = accounts.GetPage<Account>(page, itemsPerPage);

        //        //Stick stuff into the model/ session so we can check for it on the next page request
        //        //var criteria = new AccountSearchCriteria(search_status, search_FirstName, search_LastName, search_email);
        //        //AccountSearchModel model = new AccountSearchModel();
        //        //model.CurrentPage = page;
        //        //model.TotalPages = totalPages;
        //        //model.CurrentCriteria = criteria;
        //        //model.CustomerList = customers;
        //        //UdshSession.AccountSearchState = model;

        //        //Return the results
        //        jsonResponse.Data = new { Accounts = returnList, CurrentPage = page, TotalPages = totalPages };
        //        return Json(jsonResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonResponse.Error = ex.Message;
        //        return Json(jsonResponse);
        //    }
        //}

        /// <summary>
        /// Get the details of a user by the user id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator")]
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Detail(int AccountId)
        //{
        //    ResponseFormatEnum format = this.GetResponseFormat(Request.QueryString);

        //    //Get the user and assign to the model
        //    Account userAccount = _accountRepository.GetUserById(AccountId);
        //    if (userAccount == null)
        //    {
        //        ErrorViewModel em = new ErrorViewModel("No user with Id " + AccountId.ToString() + " was found.", null, "UserController", "Index");
        //        return this.GetView(format, "Error", "~/Views/Shared/ErrorPartial.ascx", em);
        //    }

        //    TIPApplicationState appSession = (TIPApplicationState)Session[DRCOGApp.SessionIdentifier];
        //    if (appSession == null || appSession.ApplicationState.CurrentUser == null)
        //    {
        //        //TODO: How should this be handled for JSON requests?
        //        ErrorViewModel em = new ErrorViewModel("Invalid session.", null, "UserController", "Index");
        //        return this.GetView(format, "Error", "~/Views/Shared/ErrorPartial.ascx", em);
        //    }

        //    var accountDetail = new AccountDetailModel
        //        {
        //            AccountDetail = userAccount
        //        };
        //    var viewModel = new AccountViewModel(accountDetail, null);
        //        //{
        //        //    //CurrentUser = appSession.CurrentUser
        //        //};
            
        //    return this.GetView(format, "AccountDetailPartial", "~/Views/Account/Partials/AccountDetailPartial.ascx", viewModel);
        //}


        /// <summary>
        /// Display the profile view
        /// </summary>
        /// <returns></returns>
        //[RoleAuth]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Profile()
        {
            //Get the current user's ID
            var viewModel = new ProfileViewModel
            {
                Profile = new ProfileModel()                
            };
            viewModel.Profile.CurrentUser = CurrentSessionApplicationState.CurrentUser;

            return View(viewModel);
        }

        /// <summary>
        /// Update the password
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <remarks>The only thing a user can do in this version is change their password</remarks>
        /// <returns></returns>
        //[RoleAuth]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Profile(string currentPassword, string newPassword, string confirmPassword)
        //{
        //    TIPApplicationState appSession = (TIPApplicationState)Session[DRCOGApp.SessionIdentifier];            
        //    // Validate the user
        //    var viewModel = new ProfileViewModel 
        //        {                    
        //            Profile = new ProfileModel()
        //        };
        //    viewModel.Profile.CurrentUser = appSession.ApplicationState.CurrentUser;

        //    if (currentPassword == "")
        //    {
        //        ModelState.AddModelError("changePwdForm", "The old password is incorrect.");
        //        return View(viewModel);
        //    }

        //    if (newPassword == "" || confirmPassword == "")
        //    {
        //        ModelState.AddModelError("changePwdForm", "New Password can not be an empty string");
        //        return View(viewModel);
        //    }

        //    string hashedPwd = Security.EncodePassword(currentPassword);
        //    if (hashedPwd != appSession.ApplicationState.CurrentUser.PasswordHash)
        //    {                                             
        //        ModelState.AddModelError("changePwdForm", "The old password is incorrect.");
        //        return View(viewModel);
        //    }

        //    if (newPassword != confirmPassword)
        //    {
        //        ModelState.AddModelError("changePwdForm", "The new password does not match the confirm new password.");
        //        return View(viewModel);
        //    }

        //    //Update password in database...
        //    _accountRepository.ChangePassword(appSession.ApplicationState.CurrentUser.AccountId, Security.EncodePassword(newPassword));

        //    //update the hash stored in session 
        //    appSession.ApplicationState.CurrentUser.PasswordHash = Security.EncodePassword(newPassword);

        //    //Redirect to the home page and call it good.
        //    return RedirectToAction("PasswordChanged", "Account");
        //}

        //[RoleAuth()]
        public ActionResult PasswordChanged()
        {
            //just a simple confirmation view. Could be done via a popup
            return View();
        }

        /// <summary>
        /// Returns the RoleDialog view
        /// </summary>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator")]
        //public ActionResult GetRoleDialog()
        //{
        //    ActionResult result = null;

        //    //ApplicationState appSession = (ApplicationState)Session[DRCOGApp.SessionIdentifier];
        //    //if (appSession == null || appSession.CurrentUser == null) // || appSession.AccountDetailState == null || appSession.AccountDetailState.User == null)            
        //    //{
        //    //    throw new System.Web.HttpException(401, "Invalid session. Please log in again.");                
        //    //}
        //    //else
        //    //{
        //        var model = new RoleDialogModel();

        //        var allRoles = _accountRepository.GetRoles();
        //        var rolesDic = new Dictionary<int, string>();
        //        foreach (var role in allRoles)
        //        {
        //            rolesDic.Add(role.RoleId, role.Name);
        //        }

        //        model.RolesList = rolesDic;

        //        result = View("Dialogs/RoleDialog", model);
        //    //}

        //    return result;
        //}

        /// <summary>
        /// Reset the user's password
        /// </summary>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator")]
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult ResetPassword(int id)
        //{
        //    JsonResult result = null;

        //    try
        //    {
        //        //Get appstate and currentuser
        //        //ApplicationState appSession = (ApplicationState)Session[DRCOGApp.SessionIdentifier];

        //        //if (appSession == null || appSession.CurrentUser == null)
        //        //{
        //        //    //
        //        //    result = Json(new JsonServerResponse("Invalid session.", null), "text/json");
        //        //}
        //        //else if (appSession.CurrentUser.IsAdministrator())
        //        //{

        //        Account user = _accountRepository.GetUserById(id);
        //        if (user != null)
        //        {
        //            //reset the password
        //            string newPassword = _accountRepository.ResetPassword(id);
        //            DRCOGConfig config = DRCOGConfig.GetConfig();
        //            //email the new password to the user                    
        //            //var absoluteUri = Request.Url.AbsoluteUri;
        //            //var absolutePath = Request.Url.AbsolutePath;
        //            //var rootUrl = absoluteUri.Remove(absoluteUri.IndexOf(absolutePath));
        //            //rootUrl = (!rootUrl.EndsWith("/")) ? rootUrl : rootUrl.Substring(rootUrl.Length - 1);
        //            string url = config.ChangePasswordBaseUrl; // +Url.Action("ChangePassword", "Account");

        //            string body = "A Administrator has reset your password. Your new password is " + newPassword + ".\r\n" + "Please follow this link to change your password.\r\n" + "Link: " + url + "?AccountID=" + id.ToString();
        //            //Get int value for the port...
        //            int port = 25;
        //            if (config.SMTPPort.HasValue)
        //            {
        //                port = config.SMTPPort.Value;
        //            }
        //            _emailService.SendEmail(config.SMTPServer,
        //                                    port,
        //                                    config.SMTPUseSSL.Value,
        //                                    config.SMTPUserName,
        //                                    config.SMTPPassword,
        //                                    config.AdminEmail,
        //                                    user.Login,
        //                                    "Your DRCOG.org Password has been Reset",
        //                                    body);
        //            result = Json(new JsonServerResponse("", new { email = user.Login }), "text/json");
        //            //_logService.Info("Password was reset for " + user.Login + " by " + User.Identity.Name);

        //        }
        //        else
        //        {
        //            //User not found!
        //            result = Json(new JsonServerResponse("User could not be updated.", null), "text/json");
        //            //_logService.Warn("An attempt was made to reset the password for a non-existant user.");
        //        }
        //        //}
        //        //else
        //        //{
        //        //    result = Json(new JsonServerResponse("Current user is not authorized to perform this action.", null), "text/json");
        //        //    _logService.Warn(appSession.CurrentUser.Login + " attempted to reset a password. System blocked this action.");
        //        //}

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logService.Fatal("Exception Occured:" + ex.ToString());
        //        return Json(new DTS.Web.MVC.JsonServerResponse(ex.Message, null), "text/json");
        //    }
        //}

        public ActionResult RecoverPassword()
        {
            return View();
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult RecoverPassword(string email)
        //{
        //    if (String.IsNullOrEmpty(email))
        //    {
        //        ModelState.AddModelError("email", new ArgumentException("email is null or empty.", "email"));
        //    }

        //    // get teh account
        //    var account = _accountRepository.GetAccountBy(email);

        //    if (account != null)
        //    {
        //        // reset the password
        //        string newPassword = Security.EncodePassword(Security.GenerateRandomPassword(8));
        //        _accountRepository.ChangePassword(account.AccountId, newPassword);

        //        // ... and email it to them
        //        DRCOGConfig config = DRCOGConfig.GetConfig();
        //        string url = config.ChangePasswordBaseUrl;
        //        string body = String.Format("Your password has been reset. Your new password is {0}.\r\nPlease follow this link to change your password.\r\nLink: {1}?AccountID={2}",
        //            newPassword,
        //            url,
        //            account.AccountId);
                
        //        _emailService.SendEmail(config.SMTPServer,
        //                                config.SMTPPort ?? 25,
        //                                config.SMTPUseSSL.Value,
        //                                config.SMTPUserName,
        //                                config.SMTPPassword,
        //                                config.AdminEmail,
        //                                email,
        //                                "Your DRCOG.org Password has been Reset",
        //                                body);

        //        // kick 'em back to the login screen so they can, you know, login
        //        TempData["message"] = "Your password has been reset.  Please check your email.";
        //        return RedirectToAction("Index", "Login");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("email", new Exception("User could not be found."));
        //        return View();
        //    }
            

        //}
    }

}
