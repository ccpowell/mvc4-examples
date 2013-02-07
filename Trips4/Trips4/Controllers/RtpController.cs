using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DRCOG.Domain;
using DRCOG.Domain.Helpers;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.ViewModels.RTP;
using DRCOG.TIP.Services.AmendmentStrategy.RTP;
using DRCOG.TIP.Services.RestoreStrategy.RTP;
using DTS.Web.MVC;

namespace Trips4.Controllers
{
    [Trips4.Filters.SessionAuthorizeAttribute]
    [Trips4.Filters.RouteData]
    public class RtpController : ControllerBase
    {
        private readonly IRtpRepository _rtpRepository;
        private readonly IRtpProjectRepository _rtpProjectRepository;
        private readonly ISurveyRepository _surveyRepository;
        private Trips4.Data.TripsRepository TripsRepository { get; set; }

        public RtpController(IRtpRepository rtpRepository, 
            IRtpProjectRepository rtpProjectRepository, 
            ISurveyRepository surveyRepository, 
            ITripsUserRepository userRepository,
            Trips4.Data.TripsRepository trepo)
            : base("RtpController", userRepository)
        {
            _rtpRepository = rtpRepository;
            _rtpProjectRepository = rtpProjectRepository;
            _surveyRepository = surveyRepository;
            TripsRepository = trepo;
        }
        private void LoadSession()
        {
            base.LoadSession(Enums.ApplicationState.RTP);
        }
        #region RTP List/RTP

        /// <summary>
        /// Returns a list of the current Plans
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string year)
        {
            LoadSession();
            //appstate.State.CurrentProgram = Enums.ApplicationState.RTP;//"Regional Transportation Plan";
            // if coming in fresh from main menu then take to current plan
            if (String.IsNullOrEmpty(year) || year == "0")
            {
                year = _rtpRepository.GetCurrentRtpPlanYear();
                return RedirectToAction("Dashboard", new { @year = year });
            }
            var viewModel = _rtpRepository.GetRtpListViewModel();
            viewModel.RtpSummary = _rtpRepository.GetSummary(year);
            //reset search model when going back to index.
            CurrentSessionApplicationState.ProjectSearchModel = null;

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View("Rtplist", viewModel);
        }

        /// <summary>
        /// Create a new RTP
        /// </summary>
        /// <param name="planName"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult CreateRtp(string planName)
        {
            JsonServerResponse jsr = new JsonServerResponse();
            //string newPlanName = //startYear.ToString() + "-" + endYear.ToString();
            try
            {
                _rtpRepository.CreateRtp(planName);
                jsr.Data = true;
            }
            catch (Exception ex)
            {
                jsr.Error = "An error occured creating the RTP Plan";
            }

            return Json(jsr);
        }



        #endregion

        #region RTP Dashboard Tab /RTP/Dashboard/<year>

        /// <summary>
        /// Display the RTP Dashboard
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        public ActionResult Dashboard(string year, string listType)
        {
            LoadSession();

            //RTPApplicationState appSession = (RTPApplicationState)Session[DRCOGApp.SessionIdentifier];

            if (String.IsNullOrEmpty(year))
            {
                year = _rtpRepository.GetCurrentRtpPlanYear();
                return RedirectToAction("Dashboard", new { @year = year });
            }

            CurrentSessionApplicationState.CurrentRTP = year;

            //Sets the default of the Dashboard type. I will set it to Sponsor.
            var dashboardListType =
                String.IsNullOrEmpty(listType) ? Enums.RTPDashboardListType.Sponsor : (Enums.RTPDashboardListType)Enum.Parse(typeof(Enums.RTPDashboardListType), listType, true);

            // get the view model from the repo
            var viewModel = _rtpRepository.GetDashboardViewModel(year, dashboardListType);

            // TODO: the following line can/should be set in the repo
            viewModel.ListType = dashboardListType;

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);

        }

        #endregion

        #region RTP Project List

        public ActionResult ResetSearchModel(string year)
        {
            LoadSession();
            CurrentSessionApplicationState.ProjectSearchModel = null;
            return RedirectToAction("ProjectList", new { year = year });
        }


        /// <summary>
        /// Returns a list of projects associated with this RTP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult ProjectList(string year, string dft, string df, int? page, int? cycleid)
        {
            LoadSession();
            if (String.IsNullOrEmpty(year))
            {

                year = _rtpRepository.GetCurrentRtpPlanYear();
                return RedirectToAction("Dashboard", new { @year = year });
            }


            //Make a ProjectViewModel object from the search criteria
            var projectSearchModel = new RTPSearchModel();

            //If there is a 'df' dashboard filter, then the Session search criteria are reset.
            if (df != null)
            {
                //Reset Session search criteria
                CurrentSessionApplicationState.ProjectSearchModel = null;

                //Assign dashboard search filter criteria
                projectSearchModel.RtpYear = year;

                //ToDo: convert 'df' we need to know what DashboardListType ('dft') it is (Sponsor = 1,ProjectType = 2,AmendmentStatus = 3)
                //ToDo: Assign the 'df' variable to projectSearchModel
                switch (dft)
                {
                    case "AmendmentStatus":
                        projectSearchModel.AmendmentStatus = df;
                        break;
                    case "Sponsor":
                        projectSearchModel.SponsorAgency = df;
                        break;
                    case "ProjectType":
                        projectSearchModel.ProjectType = df;
                        break;
                    case "ImprovementType":
                        projectSearchModel.ImprovementType = df;
                        break;
                    case "SponsorWithTipid":
                        projectSearchModel.SponsorAgency = df;
                        projectSearchModel.RequireTipId = true;
                        break;
                }

                //Assume from dashboard that we only want active projects.
                //projectSearchModel.VersionStatusId = rtpSummary.IsPending ? (int)RTPVersionStatus.Pending : (int)RTPVersionStatus.Active;
            }
            else
            {
                //Check to see if there is a projectSearchModel in Session. If not, then we have nt selected a dashboard or project search tab option.
                var sm = CurrentSessionApplicationState.ProjectSearchModel as RTPSearchModel;
                if (sm != null)
                {
                    //Pull ProjectSearchModel from session and use
                    projectSearchModel = sm;
                }
                else
                {
                    //Create search using RTPYear and Active Version only (default).
                    projectSearchModel.RtpYear = year;
                    //projectSearchModel.VersionStatusId = rtpSummary.IsPending ? (int)RTPVersionStatus.Pending : (int)RTPVersionStatus.Active;
                }
            }

            projectSearchModel.CycleId = cycleid ?? 0;

            //Before passing the ProjectSearchModel, make sure it is validated
            projectSearchModel = this.ValidateSearchData((RTPSearchModel)projectSearchModel, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram));

            //DTS NOTE: We don't fetch the model from the Repo directly because we will have to handle some complex criteria and filtering options
            var viewModel = new ProjectListViewModel();
            viewModel.RtpSummary = cycleid != null ? _rtpRepository.GetSummary(year, (int)cycleid) : _rtpRepository.GetSummary(year);
            //if (projectSearchModel.CycleId.Equals(default(int))) { projectSearchModel.CycleId = viewModel.RtpSummary.Cycle.Id; }
            if (viewModel.RtpSummary.Cycle.StatusId.Equals((int)Enums.RTPCycleStatus.Pending)) projectSearchModel.ShowCancelledProjects = true;
            viewModel.ProjectList = _rtpRepository.GetRTPProjects(projectSearchModel);
            viewModel.ListCriteria = df;
            viewModel.ListType = dft;
            if (viewModel.ProjectList.Count > 1000)
            {
                int originalCount = viewModel.ProjectList.Count;
                viewModel.ProjectList = viewModel.ProjectList.Take(1000).ToList();
                ViewData["ShowMessage"] = "Your results exceeded 1000 records. Please refine your search to narrow your results";
            }
            //viewModel.RestorableProjectList = _rtpRepository.GetRestoreProjectList(_rtpRepository.GetYearId(year, Enums.TimePeriodType.PlanYear));
            //Now save this projectSearchModel (for future searchs)
            CurrentSessionApplicationState.ProjectSearchModel = projectSearchModel;

            viewModel.ReturnUrl = Request["ReturnUrl"] ?? String.Empty;

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            pp.Add("CurrentCycleId", viewModel.RtpSummary.Cycle.Id);
            pp.Add("PreviousCycleId", viewModel.RtpSummary.Cycle.PriorCycleId);
            pp.Add("NextCycleId", viewModel.RtpSummary.Cycle.NextCycleId);
            pp.Add("RtpPlanYear", viewModel.RtpSummary.RtpYear);
            pp.Add("RtpPlanYearId", viewModel.RtpSummary.RTPYearTimePeriodID);
            SetPageParameters(pp);

            return View(viewModel);
        }

        public JsonResult GetAvailableRestoreProjects(string plan)
        {

            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _rtpRepository.GetRestoreProjectList(_rtpRepository.GetYearId(plan, Enums.TimePeriodType.PlanYear));
                result.Add(new SelectListItem { Text = "", Value = "" });
                availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.RtpYear + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        public JsonResult CreateSurvey(int planId, int surveyName)
        {
            var surveyId = default(int);
            try
            {
                surveyId = _surveyRepository.CreateSurvey(planId, surveyName.ToString());
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
                message = "Survey created successfully."
                ,
                error = "false"
                ,
                data = surveyId
            });
        }

#if obsolete

        public JsonResult GetAmendableProjects(string plan, int cycleId)
        {
            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _rtpRepository.GetAmendableProjects(_rtpRepository.GetYearId(plan, Enums.TimePeriodType.PlanYear), cycleId, true, true).ToList();
                result.Add(new SelectListItem { Text = "", Value = "" });
                availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Cycle.Name + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        // TODO: moved to /operation/misc/GetAmendablePendingProjects
        public JsonResult GetAmendablePendingProjects(string plan, int cycleId)
        {

            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _rtpRepository.GetAmendableProjects(_rtpRepository.GetYearId(plan, Enums.TimePeriodType.PlanYear), cycleId, false).ToList();
                return Json(availableProjects);
                //result.Add(new SelectListItem { Text = "", Value = "" });
                //availableProjects.ToList();//.ForEach(x => { result.Add(new SelectListItem { Text = x.RtpYear + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                //result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }
#endif
        [Trips4.Filters.SessionAuthorizeAttribute]
        public JsonResult GetPlanCycles(string plan)
        {
            var result = new List<Cycle>();

            try
            {
                result = _rtpRepository.GetPlanCycles(_rtpRepository.GetYearId(plan, Enums.TimePeriodType.PlanYear)).ToList();
            }
            catch (Exception ex)
            {
                result.Clear();
                return Json(new { response = result });
            }
            return Json(result);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DeleteProjectVersion(int projectVersionId)
        {
            bool result = false;
            try
            {
                result = _rtpProjectRepository.DeleteProjectVersion(projectVersionId/*, Enums.RTPAmendmentStatus.Submitted*/);
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
                message = "Project successfully removed."
                ,
                error = "false"
            });

        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult Restore(string plan, int id)
        {
            RtpSummary result = null;
            try
            {
                IRestoreStrategy strategy = new RestoreStrategy(this._rtpProjectRepository, id).PickStrategy();
                result = (RtpSummary)strategy.Restore(plan);
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
                    //data = new
                    //{
                    //    RtpYear = result.RtpYear,
                    //    Title = result.Title,
                    //    ProjectVersionId = result.ProjectVersionId,
                    //    COGID = result.COGID,
                    //    AmendmentStatus = result.AmendmentStatus,
                    //    ImprovementType = result.ImprovementType,
                    //    SponsorAgency = result.SponsorAgency
                    //}
                ,
                message = "Project successfully restored to " + plan + "."
                ,
                error = "false"
            });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult SetPlanCurrent(int timePeriodId)
        {
            int result = default(int);
            //CycleAmendment amendment = new CycleAmendment() { ProjectVersionId = projectVersionId, Id = cycleId };
            try
            {
                _rtpRepository.SetPlanCurrent(timePeriodId);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = "Setting plan to current was not successful."
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
                message = "Plan successfully set to current."
                ,
                error = "false"
            });

        }

#if obsolete
        // TODO: this has moved to /operation/misc/RtpAmendProjects
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult Amend(int projectVersionId, int cycleId)
        {
            int result = default(int);
            CycleAmendment amendment = new CycleAmendment() { ProjectVersionId = projectVersionId, Id = cycleId };
            try
            {
                IAmendmentStrategy strategy = new AmendmentStrategy(_rtpProjectRepository, amendment).PickStrategy();
                result = strategy.Amend();
                //result = RtpProjectRepository.DeleteProjectVersion(projectVersionId, RTPAmendmentStatus.Submitted);
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
                message = "Project successfully amended."
                ,
                error = "false"
            });

        }
#endif
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult CreateProject(string projectName, string facilityName, string plan, int sponsorOrganizationId, int? cycleId)
        {
            JsonServerResponse jsr = new JsonServerResponse();
            int projectVersionId = 0;
            try
            {
                projectVersionId = _rtpRepository.CreateProject(projectName, facilityName, plan, sponsorOrganizationId, cycleId);
            }
            catch (Exception ex)
            {
                jsr.Error = "An error occured creating the RTP Project";
                //_logService.Warn("Create new TIP Project failed for ProjectName='" + projectName + "'");
                return Json(jsr);
            }
            return new JsonResult
            {
                Data = projectVersionId
            };
        }

        #endregion

        #region Project Search

        /// <summary>
        /// Get the ProjectSearch view.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult ProjectSearch(string year)
        {
            LoadSession();
            var viewModel = new ProjectSearchViewModel();

            viewModel = _rtpRepository.GetProjectSearchViewModel(year, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram));
            viewModel.ProjectSearchModel = this.GetProjectSearchModel();

            viewModel.RtpSummary.RtpYear = year;

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);
        }

        private RTPSearchModel GetProjectSearchModel()
        {
            //This method will check to see if a ProjectSearchModel is already in exsitance in the Session object.
            //If so, it will copy it to the current object. If not, then defaults will be returned.

            //ToDo: Add a variable for the application, so that each application can specifiy its own defaults?

            var result = new RTPSearchModel();
            LoadSession();
            //Get a reference to session object
            //ApplicationState appSession = this.GetSession();

            if ((CurrentSessionApplicationState.ProjectSearchModel as RTPSearchModel) != null)
            {
                result = (RTPSearchModel)CurrentSessionApplicationState.ProjectSearchModel;
            }
            else
            {
                _rtpRepository.SetProjectSearchDefaults(result);
                //Just return some general defaults for now
                result.AmendmentStatusID = null;
                result.COGID = "";
                result.ProjectName = "";
                result.ProjectType = "";
                result.SponsorAgencyID = null;
                result.RtpID = "";
                result.ImprovementTypeID = null;
                result.VersionStatusId = (int)Enums.RTPVersionStatus.Active;
            }

            return result;
        }

        /// <summary>
        /// Store the criteria and redirect to the ProjectList
        /// which will then apply the new criteria.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProjectSearch(ProjectSearchViewModel model)
        {
            LoadSession();

            //Save search options to session
            if (CurrentSessionApplicationState.ProjectSearchModel != null) CurrentSessionApplicationState.ProjectSearchModel = null;
            //if (Session["ProjectSearchModel"] != null) Session.Remove("ProjectSearchModel");
            CurrentSessionApplicationState.ProjectSearchModel = model.ProjectSearchModel;
            //Session.Add("ProjectSearchModel", model.ProjectSearchModel);

            //Redirect to the project list. Determine RTP Year (text) for redirect from appSession
            return new RedirectToRouteResult(
                              new RouteValueDictionary {                              
                              { "controller", "RTP" },
                              { "action", "ProjectList" },
                              { "year", CurrentSessionApplicationState.CurrentRTP }                             
                        });
        }

        public JsonResult GetPlanScenarios(int planYearId)
        {
            var result = new List<SelectListItem>();

            try
            {
                var planScenarios = _rtpRepository.GetPlanScenarios(planYearId);
                result.Add(new SelectListItem { Text = "", Value = "" });
                planScenarios.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Scenarios found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        public JsonResult GetPlanScenariosForCurrentCycle(int planYearId)
        {
            var result = new List<SelectListItem>();

            try
            {
                var planScenarios = _rtpRepository.GetPlanScenariosForCurrentCycle(planYearId);
                result.Add(new SelectListItem { Text = "", Value = "" });
                planScenarios.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Scenarios found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        #endregion

        #region RTP Amendments Tab

        public ActionResult Amendments(string year)
        {
            var viewModel = new RtpBaseViewModel();
            viewModel.RtpSummary.RtpYear = year;

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);
        }

        #endregion

        #region RTP Status /RTP/<year>/Status/

        /// <summary>
        /// Display the status for a RTP
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public ActionResult Status(string year)
        {
            LoadSession();
            if (String.IsNullOrEmpty(year) || year == "0")
            {
                year = _rtpRepository.GetCurrentRtpPlanYear();
                CurrentSessionApplicationState.CurrentRTP = year;
            }
            // get the view model from the repo
            var viewModel = _rtpRepository.GetRtpStatusViewModel(year);
            viewModel.RtpSummary = _rtpRepository.GetSummary(year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);
            
            return View("Status", viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator, Survey Administrator")]
        public JsonResult SetSurveyDates(DRCOG.Domain.Models.Survey.Survey model)
        {
            try
            {
                _surveyRepository.SetSurveyDates(model);
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
                message = "Survey successfully updated."
                ,
                error = "false"
            });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator, Survey Administrator")]
        public JsonResult GetSurveyDates(DRCOG.Domain.Models.Survey.Survey model)
        {
            try
            {
                Survey datesPartial = _surveyRepository.GetSurveyDates(model);
                model.OpeningDate = datesPartial.OpeningDate;
                model.ClosingDate = datesPartial.ClosingDate;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = "Survey dates could not be retrieved. An error has been logged."
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Survey dates successfully retreived."
                ,
                data = model
                ,
                error = "false"
            });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult CloseSurveyNow(DRCOG.Domain.Models.Survey.Survey model)
        {
            try
            {
                _surveyRepository.CloseSurveyNow(model);
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
                message = "Survey successfully updated."
                ,
                error = "false"
            });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult OpenSurveyNow(DRCOG.Domain.Models.Survey.Survey model)
        {
            try
            {
                _surveyRepository.OpenSurveyNow(model);
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
                message = "Survey successfully updated."
                ,
                error = "false"
            });
        }


        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateTimePeriodStatusId(int timePeriodId, int statusId)
        {
            string error = String.Empty;
            try
            {
                error = _rtpRepository.UpdateTimePeriodStatusId(timePeriodId, statusId);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Project Financial Record Detail successfully added."
                ,
                error = "false"
            });
        }

        /// <summary>
        /// Add a cycle to the plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="cycleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddCycle(string plan, int cycleId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _rtpRepository.AddCycleToTimePeriod(plan, cycleId);
            return Json(jsr);
        }

        /// <summary>
        /// Remove a cycle
        /// </summary>
        /// <param name="cycleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DropCycle(int cycleId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _rtpRepository.RemoveCycleFromTimePeriod(cycleId);
            if (!jsr.Error.Equals(""))
            {
                jsr.Error = "Cycle is in use in the current Plan. It can not be removed.";
            }
            return Json(jsr);
        }

        /// <summary>
        /// Create a Cycle
        /// </summary>
        /// <param name="cycle"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult CreateCycle(string cycle)
        {
            string error = _rtpRepository.CreateCycle(cycle);
            if (!String.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    message =  error
                    ,
                    error = "true"
                });
            }
            return Json(new
            {
                message = "Cycle successfully added."
                ,
                error = "false"
            });
        }

        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateCycleName(int cycleId, string cycle)
        {
            string error = String.Empty;
            try
            {
                error = _rtpRepository.UpdateCycleName(cycleId, cycle);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Successfully updated."
                ,
                error = "false"
            });
        }

        public JsonResult GetSponsorOrganizations(string plan)
        {
            var result = new List<SelectListItem>();
            var sponsorOrganizations = _rtpRepository.GetCurrentTimePeriodSponsorAgencies(plan, Enums.ApplicationState.RTP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            sponsorOrganizations.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            return new JsonResult
            {
                Data = result
            };
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult SetActiveCycle(int cycleId, int timePeriodId)
        {
            string data = String.Empty;
            try
            {
                data = _rtpRepository.SetActiveCycle(cycleId, timePeriodId);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = String.IsNullOrEmpty(data) ? "Changes could not be stored." : data
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Project Financial Record Detail successfully added."
                ,
                error = "false"
                ,
                data = data
            });
        }

        public JsonResult GetPlanAvailableProjects(int planId, int cycleId)
        {

            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _rtpRepository.GetPlanAvailableProjects(planId, cycleId);
                result.Add(new SelectListItem { Text = "", Value = "" });
                availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
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
        public JsonResult UpdateCycleSort(string cycles)
        {

            string error = String.Empty;
            try
            {
                _rtpRepository.UpdateTimePeriodCycleOrder(cycles);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Project Financial Record Detail successfully added."
                ,
                error = "false"
            });
        }

        //[Trips4.Filters.SessionAuthorizeAttribute]
        //public ActionResult Amend(StatusViewModel model)
        //{
        //    var amendment = model.Cycle;
        //    //this.Amend(amendment);
        //    return View();
        //    //return RedirectToAction("Funding", new { controller = "Project", guid = projectVersionId, Message = "Amendment processed successfully." });
        //}

        //[Trips4.Filters.SessionAuthorizeAttribute]
        //public ActionResult Amend(Cycle cycle)
        //{
        //    foreach (CycleAmendment amendment in cycle.Projects)
        //    {
        //        IAmendmentStrategy strategy = new AmendmentStrategy(RtpProjectRepository, amendment).PickStrategy();
        //        //strategy.Amend();
        //    }
        //    return View();
        //}

        ///// <summary>
        ///// Get the Reports View
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //[Trips4.Filters.SessionAuthorizeAttribute]
        //public ActionResult Reports(string year)
        //{
        //    //Create the ViewModel
        //    ReportsViewModel model = new ReportsViewModel();
        //    model.TipSummary = _rtpRepository.GetTIPSummary(year);
        //    return View("reports", model);
        //}

        #endregion

        #region RTP Eligible Agencies Tab

        /// <summary>
        /// Display the eligible agencies lists for a TIP
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Agencies(string year)
        {

            //get the mode from the repo
            SponsorsViewModel viewModel = _rtpRepository.GetSponsorsViewModel(year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);
        }

        /// <summary>
        /// Callback to see if an agency can be dropped from the tip
        /// </summary>
        /// <param name="year"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        //[Trips4.Filters.SessionAuthorizeAttribute]
        //public ActionResult CheckAgency(string year, int agencyId)
        //{
        //    if (_tipRepository.CanAgencyBeDropped(year, agencyId))
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //}

        /// <summary>
        /// Update the eligible agencies associated with this RTP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateAgencies(string plan, List<int> added, List<int> removed)
        {
            if (added == null)
            {
                added = new List<int>();
            }
            if (removed == null)
            {
                removed = new List<int>();
            }
            //Send the two lists of ID's to the Repo
            try
            {
                _rtpRepository.UpdateEligibleAgencies(plan, added, removed);
                JsonServerResponse jsr = new JsonServerResponse();
                jsr.Data = true;
                return Json(jsr);
            }
            catch (Exception ex)
            {
                JsonServerResponse jsr = new JsonServerResponse();
                jsr.Error = ex.Message;
                return Json(jsr);
            }
        }

        /// <summary>
        /// Add an agency to the eligible agencies list
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddEligibleAgency(string plan, int agencyId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _rtpRepository.AddAgencyToTimePeriod(plan, agencyId, CurrentSessionApplicationState.CurrentProgram);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an agency from the eligible agencies list
        /// if the agency does not sponsor any projects in the
        /// tip
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DropEligibleAgency(string plan, int agencyId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _rtpRepository.DropAgencyFromTimePeriod(plan, agencyId, CurrentSessionApplicationState.CurrentProgram);
            if (!jsr.Error.Equals(""))
            {
                jsr.Error = "That agency sponsors projects in the current Plan. They can not be removed.";
            }
            return Json(jsr);
        }

        #endregion

        #region RTP FundingList Tab

        public ActionResult FundingList(string year, int? page)
        {

            var viewModel = new FundingSourceListViewModel();
            viewModel.RtpSummary = _rtpRepository.GetSummary(year);
            viewModel.FundingSources = _rtpRepository.GetFundingSources(year); //.AsPagination(page.GetValueOrDefault(1), 10);                               

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);
        }

        #endregion

        #region Plan Cycles Tab

        public ActionResult PlanCycles(string year)
        {
            var viewModel = new PlanCyclesViewModel();
            viewModel.RtpSummary = _rtpRepository.GetSummary(year);
            int id = TripsRepository.GetRtpPlanYearId(year);
            viewModel.Cycles = TripsRepository.GetRtpPlanCycles(id);
            viewModel.ExistsNewPlanCycle = (null != viewModel.Cycles.FirstOrDefault(c => c.Status == "New"));

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);
            
            return View(viewModel);
        }

        #endregion

        /// <summary>
        /// Get the Reports View, or redirect the user if he is not authorized
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult Reports(string year)
        {
            LoadSession();

            if (!System.Web.Security.Roles.IsUserInRole("Administrator") &&
                !System.Web.Security.Roles.IsUserInRole("RTP Administrator"))
            {
                Redirect("http://www.drcog.org/index.cfm?page=RegionalTransportationPlan(RTP)");
            }

            //Create the ViewModel
            ReportsViewModel viewModel = TripsRepository.GetReportsViewModel(year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", viewModel.RtpSummary.RtpYear);
            SetPageParameters(pp);

            return View(viewModel);
        }


        public ActionResult DownloadModelerExtract(int timePeriodId, int? excludeOpenBefore)
        {
            var results = TripsRepository.GetRtpModelerExtractDocument(timePeriodId, excludeOpenBefore);
            return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "RTP_ModelerExtract.xlsx");
        }

        #region PRIVATE HELPERS

        private RTPSearchModel ValidateSearchData(RTPSearchModel projectSearchModel, string currentProgram)
        {
            //Check completeness of TipYear
            if ((projectSearchModel.RtpYearID == null) && (projectSearchModel.RtpYear != null))
            {
                //Lookup the RtpYearID
                projectSearchModel.RtpYearID = _rtpRepository.GetYearId(projectSearchModel.RtpYear, Enums.TimePeriodType.PlanYear);
            }

            if ((projectSearchModel.RtpYearID != null) && (projectSearchModel.RtpYear == null))
            {
                //Lookup the TipYear
                projectSearchModel.RtpYear = _rtpRepository.GetYear((int)projectSearchModel.RtpYearID);
            }

            //Check completeness of SponsorAgency
            if ((projectSearchModel.SponsorAgencyID == null) && (projectSearchModel.SponsorAgency != null))
            {
                //Lookup the SponsorAgencyID
                projectSearchModel.SponsorAgencyID = _rtpRepository.GetSponsorAgencyID(projectSearchModel.SponsorAgency);
            }

            if ((projectSearchModel.SponsorAgencyID != null) && (projectSearchModel.SponsorAgency == null))
            {
                //Lookup the SponsorAgency
                projectSearchModel.SponsorAgency = _rtpRepository.GetSponsorAgency(projectSearchModel.SponsorAgencyID);
            }

            string statusType = "";
            switch (currentProgram)
            {
                case "Transportation Improvement Plan":
                    statusType = "TIP Amendment Status";
                    break;
                case "Regional Transportation Plan":
                    statusType = "RTP Amendment Status";
                    break;
                case "Transportation Improvement Survey":
                    statusType = "Survey Amendment Status";
                    break;
                default:
                    statusType = "TIP Amendment Status"; // If something goes wrong, assume TIP
                    break;
            }

            //Check completeness of AmendmentStatus
            if ((projectSearchModel.AmendmentStatusID == null) && (projectSearchModel.AmendmentStatus != null))
            {
                //Lookup the AmendmentStatusID
                projectSearchModel.AmendmentStatusID = _rtpRepository.GetStatusID(projectSearchModel.AmendmentStatus, statusType);
            }

            if ((projectSearchModel.AmendmentStatusID != null) && (projectSearchModel.AmendmentStatus == null))
            {
                //Lookup the AmendmentStatus
                projectSearchModel.AmendmentStatus = _rtpRepository.GetStatus(projectSearchModel.AmendmentStatusID, statusType);
            }

            //Check completeness of ImprovementType
            if ((projectSearchModel.ImprovementTypeID == null) && (projectSearchModel.ImprovementType != null))
            {
                //Lookup the ImprovementTypeID
                projectSearchModel.ImprovementTypeID = _rtpRepository.GetImprovementTypeID(projectSearchModel.ImprovementType);
            }

            if ((projectSearchModel.ImprovementTypeID != null) && (projectSearchModel.ImprovementType == null))
            {
                //Lookup the ImprovementType
                projectSearchModel.ImprovementType = _rtpRepository.GetImprovementType(projectSearchModel.ImprovementTypeID);
            }

            //Check completeness of ProjectType
            if ((projectSearchModel.ProjectTypeID == null) && (projectSearchModel.ProjectType != null))
            {
                //Lookup the ProjectTypeID
                projectSearchModel.ProjectTypeID = _rtpRepository.GetProjectTypeID(projectSearchModel.ProjectType);
            }

            if ((projectSearchModel.ProjectTypeID != null) && (projectSearchModel.ProjectType == null))
            {
                //Lookup the ProjectType
                projectSearchModel.ProjectType = _rtpRepository.GetProjectType(projectSearchModel.ProjectTypeID);
            }

            //Check completeness of PlanType
            if ((projectSearchModel.PlanTypeId == 0) && (projectSearchModel.PlanType != null))
            {
                //Lookup the PlanTypeID
                projectSearchModel.PlanTypeId = _rtpRepository.GetCategoryId(projectSearchModel.PlanType);
            }

            if ((projectSearchModel.PlanTypeId > 0) && (projectSearchModel.PlanType == null))
            {
                //Lookup the PlanType
                projectSearchModel.PlanType = _rtpRepository.GetCategory(projectSearchModel.PlanTypeId);
            }

            return projectSearchModel;
        }

        #endregion
    }
}