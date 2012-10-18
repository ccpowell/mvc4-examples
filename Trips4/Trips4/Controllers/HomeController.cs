using System;
using System.Web.Mvc;
using DRCOG.Domain.Interfaces;
//using Trips4.Filters;
using DRCOG.Domain.ViewModels;
using Trips4.Utilities.ApplicationState;
using DRCOG.Domain.Security;
using DRCOG.TIP.Services;
using DRCOG.Domain;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using DRCOG.Common.Web.MvcSupport.Attributes;
using System.Web;
using System.Configuration;

namespace Trips4.Controllers
{
    /// <summary>
    /// Main splash page for the application
    /// </summary>
    [HandleError]
    [Trips4.Filters.SessionAuthorizeAttribute]
    [Trips4.Filters.SessionAuthorizeAttribute]
    //[RemoteRequireHttps]
    public class HomeController : ControllerBase
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref=this.ControllerName/> class.
        /// </summary>
        public HomeController(IUserRepositoryExtension userRepository)
            : base("HomeController", userRepository)
        {
                        
        }

        #endregion

        private void LoadSession()
        {
            base.LoadSession(Enums.ApplicationState.Default);
        }


        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Index()
        {
            LoadSession();
            //ISitePrincipal user = UserService.GetCurrentUser();

            const string methodName = "Index";
            //#region Log callback
            //this.Logger.LogCallbackInfo(this.ControllerName, methodName, "UpdateStatus called...");
            //#endregion


            //Based on Survey being open, do we need to override returnUrl? -DBD 06/22/2012 at request of Lawrence
            string redirectUrl = "";

            //Is survey open?
            DRCOG.Data.SurveyRepository surveyRepo = new DRCOG.Data.SurveyRepository();
            DRCOG.Domain.Models.Survey.Survey currentSurvey = surveyRepo.GetCurrentSurvey();
            surveyRepo = null;

            if ((currentSurvey.OpeningDate <= DateTime.Now) && (DateTime.Now <= currentSurvey.ClosingDate))
            {
                //Survey is open, Are they a sponsor?
                if ((this.CurrentSessionApplicationState.CurrentUser != null) && (this.CurrentSessionApplicationState.CurrentUser.SponsorOrganizationName != null))
                {
                    //On development, the TRIPS/ is not needed. In production it is.
                    redirectUrl = ConfigurationManager.AppSettings["SiteIdentifier"] + @"/Survey/" + currentSurvey.Name + @"/ProjectList?df=" + HttpUtility.HtmlEncode(this.CurrentSessionApplicationState.CurrentUser.SponsorOrganizationName) + @"&dft=Sponsor";
                    return Redirect(redirectUrl);
                }
            }

            try
            {
                ApplicationState appSession = this.GetSession();
                var viewModel = new HomeViewModel();
                //viewModel.CurrentUser = appSession.CurrentUser;
                //Logger.Log.Info("Identity: " + User.Identity.Name);
                return View(viewModel);
            }
            catch (System.Exception ex)
            {
                //Logger.LogMethodError(this.ControllerName, methodName, "no params", ex);
                return HandleErrorHtml(ex, methodName);
            }
        }
       
    }
}
