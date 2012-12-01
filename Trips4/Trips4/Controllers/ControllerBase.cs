using System;
using System.Text;
using System.Web.Mvc;
using DTS.Web.MVC;
using DRCOG.Domain.Interfaces;
using Trips4;
//using Trips4.Filters;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain;
using Trips4.Utilities.ApplicationState;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services;
using DRCOG.Domain.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
//using DRCOG.Common.Services.MemberShipServiceSupport;
using System.Linq;
using System.Xml.Linq;
//using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using System.Data.SqlClient;
using System.Data;
using DRCOG.Domain.Security;
using System.Web.Configuration;
using System.Web.Security;
using System.Web;

namespace Trips4.Controllers
{

    /// <summary>
    /// Controller base class containing common functionalities.
    /// </summary>
    public class ControllerBase : Controller
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private const string _jsonResultContentType = "text/json-comment-filtered";
        private string _name = string.Empty;
        protected readonly ITripsUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase"/> class.
        /// </summary>
        public ControllerBase()
        {
            Logger.Debug("ControllerBase constructor was called without parameters.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ControllerBase(string controllerName, ITripsUserRepository userRepository)
        {
            _name = controllerName;
            _userRepository = userRepository;
        }


        public ITripsUserRepository UserService
        {
            get
            {
                return _userRepository;
            }
        }

        protected ApplicationState CurrentSessionApplicationState
        {
            get { return GetSession(); }
        }


        protected void LoadSession(DRCOG.Domain.Enums.ApplicationState applicationState = DRCOG.Domain.Enums.ApplicationState.Default)
        {
            ApplicationState appstate = this.GetSession();
            if (appstate == null)
            {
                appstate = GetNewSession();
            }
            appstate.SetStateType(applicationState);

            SaveSession(appstate);
        }

        protected void SetAuthCookie(LogOnModel model)
        {
            int _rememberMeTimeOut, _timeOut;
            Int32.TryParse(WebConfigurationManager.AppSettings["SessionTimeOutRememberMeMinutes"], out _rememberMeTimeOut);
            Int32.TryParse(WebConfigurationManager.AppSettings["SessionTimeOutMinutes"], out _timeOut);
            int timeout = model.RememberMe ? _rememberMeTimeOut : _timeOut; // Timeout in minutes, 525600 = 365 days.
            var ticket = new FormsAuthenticationTicket(model.UserName, model.RememberMe, timeout);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
            Response.Cookies.Add(cookie);
        }

        protected ActionResult SetAuthCookie(LogOnModel model, string returnUrl)
        {
            // we need the profile first to get the username just in case the login is AD based
            CurrentSessionApplicationState.CurrentUser = _userRepository.GetUserByName(model.UserName, true);

            SetAuthCookie(model);

            this.SaveSession(CurrentSessionApplicationState);

            if (!String.IsNullOrEmpty(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }

        /// <summary>
        /// Gets the JSON result content type.
        /// </summary>
        /// <value>The type of the json result content.</value>
        protected string JsonResultContentType
        {
            get
            {
                return _jsonResultContentType;
            }
        }

        ///// <summary>
        ///// Accessor for the log helper
        ///// </summary>
        //public LogHelper Logger
        //{
        //    get
        //    {
        //        return _logger;
        //    }
        //}

        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        protected string ControllerName
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Returns the confirm dialog
        /// </summary>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult GetConfirmDialog()
        {
            return View("ConfirmDialog");
        }
#if bozo
        /// <summary>
        /// Workout the view to return...
        /// </summary>
        /// <param name="format"></param>
        /// <param name="viewName"></param>
        /// <param name="partialPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetView(DTS.Web.MVC.ResponseFormatEnum format, string viewName, string partialPath, object model)
        {

            ActionResult result = null;
            switch (format)
            {

                case ResponseFormatEnum.html:
                    result = this.View(viewName, model);
                    break;

                case ResponseFormatEnum.json:
                    JsonResult r = this.Json(model);
                    r.ContentType = "text/json-comment-filtered";
                    result = r;
                    break;

                case ResponseFormatEnum.partial:
                    result = this.View(partialPath, model);
                    break;
            }
            return result;
        }
#endif

        #region Session Methods

        /// <summary>
        /// Gets the session if it exists.
        /// </summary>
        /// <returns></returns>
        protected ApplicationState GetSession()
        {
            var sessionObject = this.Session[DRCOGApp.SessionIdentifier];
            if (sessionObject == null)
            {
                Logger.Debug("Session object was null");
                Logger.Debug("Session.IsNew = " + Session.IsNewSession);
            }
            return sessionObject as ApplicationState;
        }


        /// <summary>
        /// Saves the app state in the session. 
        /// </summary>
        protected void SaveSession(ApplicationState sessionObject)
        {
            //Session[DRCOGApp.SessionIdentifier] = null;
            Logger.Debug("Saving session " + Session.SessionID);
            Session[DRCOGApp.SessionIdentifier] = sessionObject;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ApplicationState appState = GetSession();
            if (appState != null)
            {
                ViewData["CurrentUser"] = appState.CurrentUser;
            }
            else
            {
                Logger.Debug("Session contains no ApplicationState when initialized.");
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //Logger.LogCallbackInfo(ControllerName, 
            //    filterContext.ActionDescriptor.ActionName,
            //    String.Format("{0} view requested.", filterContext.ActionDescriptor.ActionName));  
        }

        /// <summary>
        /// Clears any existing session and returns a new session object.
        /// </summary>
        /// <returns></returns>
        protected ApplicationState GetNewSession()
        {
            Logger.Debug("Initializing Session");

            Session.Clear();

            // Create application state and add to a new session
            Session.Add(DRCOGApp.SessionIdentifier, new ApplicationState());

            return this.GetSession();
        }

        //protected ApplicationState GenerateNewState(Enums.ApplicationState? applicationState)
        //{
        //    switch (applicationState)
        //    {
        //        case Enums.ApplicationState.RTP:
        //            return new RTPApplicationState(
        //        case Enums.ApplicationState.TIP:
        //            return new TIPApplicationState();
        //        default:
        //            return new ApplicationState();
        //    }
        //}

        /// <summary>
        /// Gets the current user name formatted as last name then first (Ex. "Doe, John"), or first name then last (Ex. "John Doe").
        /// </summary>
        /// <param name="lastNameFirstName">if set to <c>true</c> [last name first name].</param>
        /// <returns></returns>
        protected string GetCurrentUserFormatted(bool lastNameFirstName)
        {
            if (lastNameFirstName)
            {
                return string.Format("{0}, {1}", this.GetSession().CurrentUser.profile.LastName, this.GetSession().CurrentUser.profile.FirstName);
            }
            else
            {
                return string.Format("{0} {1}", this.GetSession().CurrentUser.profile.FirstName, this.GetSession().CurrentUser.profile.LastName);
            }
        }

        #endregion

        #region Error Handler Methods
#if bozo
        /// <summary>
        /// Error handler, returning a JsonServerResponse2 object within a JsonResult object.
        /// </summary>
        /// <param name="errorLogged">if set to <c>true</c> [error logged].</param>
        /// <returns></returns>
        protected JsonResult HandleErrorJson(bool errorLogged)
        {
            return this.HandleErrorJson(errorLogged, null, true);
        }

        /// <summary>
        /// Error handler, returning a JsonServerResponse2 object within a JsonResult object.
        /// </summary>
        /// <param name="errorLogged">if set to <c>true</c> [error logged].</param>
        /// <param name="clientErrorMessage">The client error message.</param>
        /// <param name="includeBoilerPlateMessage">if set to <c>true</c> [include boiler plate message].</param>
        /// <returns></returns>
        protected JsonResult HandleErrorJson(bool errorLogged, string clientErrorMessage, bool includeBoilerPlateMessage)
        {
            JsonServerResponse response = new JsonServerResponse();
            string boilerPlateMsg = "We're sorry, an unexpected error has occurred while attempting to process your request.";
            string errorLogMsg = "The error has been logged.";

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(clientErrorMessage))
            {
                sb.AppendLine(clientErrorMessage.Trim());
            }
            if (includeBoilerPlateMessage)
            {
                sb.AppendLine(boilerPlateMsg);
            }
            if (errorLogged)
            {
                sb.AppendLine(errorLogMsg);
            }

            response.Error = sb.ToString();

            return Json(response, this.JsonResultContentType);

        }

        /// <summary>
        /// Session timeout error handler, returning a JsonServerResponse2 object within a JsonResult object.
        /// </summary>
        /// <param name="includeBoilerPlateMessage">if set to <c>true</c> [include boiler plate message].</param>
        /// <returns></returns>
        protected JsonResult HandleSessionTimeoutErrorJson(bool includeBoilerPlateMessage)
        {
            JsonServerResponse response = new JsonServerResponse();
            string boilerPlateMsg = "Your session has expired. You must log into the application again.";

            StringBuilder sb = new StringBuilder();
            if (includeBoilerPlateMessage)
            {
                sb.AppendLine(boilerPlateMsg);
            }
            response.Error = sb.ToString();
            response.SessionError = true;

            return Json(response, this.JsonResultContentType);
        }
#endif
        /// <summary>
        /// Handles the error by returning the error view.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        protected ViewResult HandleErrorHtml(Exception ex, string methodName)
        {
            return this.HandleErrorHtml(ex, methodName, "We're sorry, an unexpected error has occurred while attempting to process your request.");
        }

        /// <summary>
        /// Handles the error by returning the error view.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        protected ViewResult HandleErrorHtml(Exception ex, string methodName, string message)
        {
            ErrorViewModel model = new ErrorViewModel(message, "Contact your site administrator to report this issue. The error has been logged.", ex, this.ControllerName, methodName);
            return View("CustomError", model);
        }


        #endregion

        /// <summary>
        /// Gets ProjectTypeId from ImprovementTypeId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected JsonResult GetImprovementTypeMatch(int id, ITransportationRepository repo)
        {
            IList<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@Id", Value = id });
            try
            {
                //var result = RtpProjectRepository.GetLookupSingle<String>("dbo.Lookup_GetProjectTypeByImprovementTypeId", "Value", sqlParms);
                var result = repo.GetLookupCollection("dbo.Lookup_GetProjectTypeByImprovementTypeId", "Id", "Value", sqlParms);

                return Json(new { id = result.First().Key, value = result.First().Value });
            }
            catch (Exception ex)
            {
                JsonServerResponse jsr = new JsonServerResponse();
                jsr.Error = ex.Message;
                return Json(jsr);
            }
        }

        /// <summary>
        /// Gets ImprovementTypeId from ProjectTypeId
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected JsonResult GetProjectTypeMatch(int id, ITransportationRepository repo)
        {
            var result = new List<SelectListItem>();

            IList<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@Id", id));
            try
            {
                var items = repo.GetLookupCollection("[dbo].[Lookup_GetImprovementTypesByProjectId]", "Id", "Value", sqlParms);
                items.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                JsonServerResponse jsr = new JsonServerResponse();
                jsr.Error = ex.Message;
                return Json(jsr);
            }
        }


        /// <summary>
        /// Parse the County Shares from the Form Parameter collection 
        /// </summary>
        /// <param name="formParams"></param>
        /// <returns></returns>
        public static Dictionary<int, CountyShareModel> ExtractCountyShares(NameValueCollection formParams)
        {
            Dictionary<int, CountyShareModel> countyShares = new Dictionary<int, CountyShareModel>();
            //Get the projectId, as we need that in order to persist the share
            int projectId = Convert.ToInt32(formParams["ProjectId"]);

            foreach (string key in formParams)
            {
                if (key.StartsWith("cty_"))
                {
                    //Parse into Dict keyed by the CountyId
                    int ctyId = Convert.ToInt32(key.Split('_')[1]);


                    CountyShareModel ctyItem = null;
                    if (countyShares.ContainsKey(ctyId))
                    {
                        ctyItem = countyShares[ctyId];
                    }
                    else
                    {
                        ctyItem = new CountyShareModel();
                        ctyItem.CountyId = ctyId;
                        ctyItem.ProjectId = projectId;
                        countyShares.Add(ctyId, ctyItem);
                    }

                    //See if we got Primary or share
                    if (key.EndsWith("_IsPrimary"))
                    {
                        //Html checkboxes are only included in form posts
                        //if they are checked. So there is no real need 
                        //for this check and there is no notification of
                        //a "false" value.
                        if (formParams[key] == "on")
                        {
                            ctyItem.Primary = true;
                        }
                    }
                    if (key.EndsWith("_share"))
                    {
                        //stored as 0 to 1 range not a percent
                        //so we divide by 100                        
                        ctyItem.Share = Convert.ToDouble(formParams[key]) / 100;
                    }
                }
            }
            return countyShares;
        }

        /// <summary>
        /// Parse the Muni Shares from the Form Parameter collection 
        /// </summary>
        /// <param name="formParams"></param>
        /// <returns></returns>
        public static Dictionary<int, MunicipalityShareModel> ExtractMuniShares(NameValueCollection formParams)
        {
            Dictionary<int, MunicipalityShareModel> shares = new Dictionary<int, MunicipalityShareModel>();
            //Get the projectId, as we need that in order to persist the share
            int projectId = Convert.ToInt32(formParams["ProjectId"]);
            foreach (string key in formParams)
            {
                if (key.StartsWith("muni_"))
                {
                    //Parse into Dict keyed by the CountyId
                    int id = Convert.ToInt32(key.Split('_')[1]);
                    //bool isPrimary = false;
                    //double share = 0;

                    MunicipalityShareModel item = null;
                    if (shares.ContainsKey(id))
                    {
                        item = shares[id];
                    }
                    else
                    {
                        item = new MunicipalityShareModel();
                        item.MunicipalityId = id;
                        item.ProjectId = projectId;
                        shares.Add(id, item);
                    }

                    //See if we got Primary or share
                    if (key.EndsWith("_IsPrimary"))
                    {
                        //Html checkboxes are only included in form posts
                        //if they are checked. So there is no real need 
                        //for this check and there is no notification of
                        //a "false" value.
                        if (formParams[key] == "on")
                        {
                            item.Primary = true;
                        }
                    }
                    if (key.EndsWith("_share"))
                    {
                        //stored as 0 to 1 range not a percent
                        //so we divide by 100                        
                        item.Share = Convert.ToDouble(formParams[key]) / 100;
                    }
                }
            }
            return shares;
        }
#if bozo
        [Trips4.Filters.SessionAuthorizeAttribute]
        public JsonResult GetAmendableSurveyProjects(int timePeriodId)
        {
            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _surveyRepository.GetAmendableProjects(timePeriodId).ToList();
                result.Add(new SelectListItem { Text = "", Value = "" });
                availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.SponsorName + ": " + x.ProjectName + ": " + x.ImprovementType, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AmendForNewSurvey(int projectVersionId, int surveyId)
        {
            try
            {
                DRCOG.Domain.Models.Survey.Instance version = new DRCOG.Domain.Models.Survey.Instance()
                {
                    ProjectVersionId = projectVersionId
                    ,
                    UpdateStatusId = (int)DRCOG.Domain.Enums.SurveyUpdateStatus.AwaitingAction
                    ,
                    TimePeriodId = surveyId
                };
                IAmendmentStrategy strategy = new DRCOG.TIP.Services.AmendmentStrategy.Survey.AmendmentStrategy(_surveyRepository, version).PickStrategy();
                projectVersionId = strategy.Amend();
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
                message = "Changes were successful."
                ,
                error = "false"
            });

        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public PartialViewResult BecomeASpsonsor(string year)
        {
            BecomeASponsorViewModel model = new BecomeASponsorViewModel();
            model.ProjectSponsorsModel = _surveyRepository.GetProjectSponsorsModel(default(int), year);

            return PartialView("BecomeASpsonsor", model);
        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public JsonResult BecomeASponsor(Profile profile)
        {
            try
            {
                if (!String.IsNullOrEmpty(profile.SponsorCode) || profile.PersonGUID.Equals(default(Guid)))
                {
                    _userRepository.LinkUserWithSponsor(profile);
                }
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
                message = "Successfully joined with sponsor contact."
                ,
                error = "false"
            });
        }
#endif
    }
}
