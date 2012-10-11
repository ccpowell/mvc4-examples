#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR	        REMARKS
 * 05/05/2010	DDavidson       1. Initial Creation. 
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DRCOG.Common.Web.MvcSupport.Attributes;
using DRCOG.Domain.Extensions;
using DRCOG.Domain.Helpers;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.ViewModels.Survey;
using DRCOG.TIP.Services;
using DRCOG.TIP.Services.AmendmentStrategy.Survey;
using Trips4.Services;
using Trips4.Utilities.ApplicationState;
using DTS.Web.MVC;


namespace Trips4.Controllers
{
    //[RemoteRequireHttps]
    public class SurveyController : ControllerBase
    {
        //private readonly ISurveyRepository _surveyRepository;
        private readonly IRtpProjectRepository _rtpProjectRepository;

        public SurveyController(ISurveyRepository surveyRepository, IRtpProjectRepository rtpProjectRepository, IUserRepositoryExtension userRepository)
            : base("SurveyController", userRepository, surveyRepository)
        {
            _rtpProjectRepository = rtpProjectRepository;
        }

        private void LoadSession()
        {
            
            //appstate = this.GetSession();
            //if (appstate == null)
            //{
            //    appstate = (ApplicationState)this.GetNewSession(Enums.ApplicationState.Survey);
            //}
            //else appstate.State.CurrentProgram = Enums.ApplicationState.Survey;

            base.LoadSession(DRCOG.Domain.Enums.ApplicationState.Survey);

            if (CurrentSessionApplicationState.CurrentUser != null)
            {
                ViewData["PersonOrganizationId"] = CurrentSessionApplicationState.CurrentUser.SponsorOrganizationId;
                ViewData["PersonId"] = CurrentSessionApplicationState.CurrentUser.profile.PersonID;
            }
            else CurrentSessionApplicationState.CurrentUser = new Person();
        }

        private bool LoadSession(Project project)
        {
            this.LoadSession();

            CurrentSessionApplicationState.CurrentUser.LastProjectVersionId = project.ProjectVersionId;
            CurrentSessionApplicationState.CurrentUser.LastSponsorContactId = project.SponsorContactId;

            return CurrentSessionApplicationState.CurrentUser.SponsorsProject(project.ProjectVersionId);

            //return project.SponsorContactId == appstate.CurrentUser.profile.PersonID ? true : false;
        }

        #region RTP Eligible Agencies Tab

        /// <summary>
        /// Display the eligible agencies lists for a TIP
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        //[RoleAuth]
        public ActionResult Agencies(string year)
        {

            //get the mode from the repo
            SponsorsViewModel viewModel = _surveyRepository.GetSponsorsViewModel(year);

            viewModel.Current.ProjectSponsorsModel = _surveyRepository.GetProjectSponsorsModel(default(int), year);

            return View(viewModel);
        }

        ///// <summary>
        ///// Update the eligible agencies associated with this RTP
        ///// </summary>
        ///// <param name="tipYear"></param>
        ///// <param name="added"></param>
        ///// <param name="removed"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        //public JsonResult UpdateAgencies(string plan, List<int> added, List<int> removed)
        //{
        //    if (added == null)
        //    {
        //        added = new List<int>();
        //    }
        //    if (removed == null)
        //    {
        //        removed = new List<int>();
        //    }
        //    //Send the two lists of ID's to the Repo
        //    try
        //    {
        //        _surveyRepository.UpdateEligibleAgencies(plan, added, removed);
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Data = true;
        //        return Json(jsr);
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Error = ex.Message;
        //        return Json(jsr);
        //    }
        //}

        /// <summary>
        /// Add an agency to the eligible agencies list
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult AddEligibleAgency(string timePeriod, int agencyId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.AddAgencyToTimePeriod(timePeriod, agencyId, DRCOG.Domain.Enums.ApplicationState.Survey);
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
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult DropEligibleAgency(string timePeriod, int agencyId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.DropAgencyFromTimePeriod(timePeriod, agencyId, DRCOG.Domain.Enums.ApplicationState.Survey);
            if (!jsr.Error.Equals(""))
            {
                jsr.Error = "That agency sponsors projects in the current Survey. They can not be removed.";
            }
            return Json(jsr);
        }

        /// <summary>
        /// Add an agency to the eligible ImprovementType list
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult AddEligibleImprovementType(string timePeriod, int improvementTypeId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.AddImprovementTypeToTimePeriod(timePeriod, improvementTypeId, DRCOG.Domain.Enums.ApplicationState.Survey);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an agency from the eligible ImprovementType list
        /// if the ImprovementType does not belong to any projects in the
        /// survey
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult DropEligibleImprovementType(string timePeriod, int improvementTypeId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.DropImprovementTypeFromTimePeriod(timePeriod, improvementTypeId, DRCOG.Domain.Enums.ApplicationState.Survey) ? "" : "The improvementType is used in the current Survey. It can not be removed.";
            //if (!jsr.Error.Equals(""))
            //{
            //    jsr.Error = "The improvementType is used in the current Survey. It can not be removed.";
            //}
            return Json(jsr);
        }

        /// <summary>
        /// Add a FundingResource to the eligible FundingResource list
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult AddEligibleFundingResource(string timePeriod, int fundingResourceId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.AddFundingResourceToTimePeriod(_surveyRepository.GetYearId(timePeriod, DRCOG.Domain.Enums.TimePeriodType.Survey), fundingResourceId);
            return Json(jsr);
        }

        /// <summary>
        /// Remove a FundingResource from the eligible FundingResource list
        /// if the FundingResource does not belong to any projects in the
        /// survey
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="improvementTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult DropEligibleFundingResource(string timePeriod, int fundingResourceId)
        {
            LoadSession();

            var jsr = new JsonServerResponse();
            jsr.Error = _surveyRepository.DropFundingResourceFromTimePeriod(_surveyRepository.GetYearId(timePeriod, DRCOG.Domain.Enums.TimePeriodType.Survey), fundingResourceId, DRCOG.Domain.Enums.ApplicationState.Survey) ? "" : "The funding resource is used in the current Survey. It can not be removed.";
            return Json(jsr);
        }

        #endregion

        #region Survey Status

        
        

        #endregion

        #region Survey List/Survey

        /// <summary>
        /// Returns a list of the current Surveys
        /// </summary>
        /// <returns></returns>
        //[RoleAuth]
        public ActionResult Index(string year)
        {
            LoadSession();
            // if coming in fresh from main menu then take to current survey
            if (String.IsNullOrEmpty(year) || year == "0") 
            {
                year = _surveyRepository.GetCurrentSurveyYear();
                if (CurrentSessionApplicationState.CurrentUser != null && !CurrentSessionApplicationState.CurrentUser.profile.PersonGUID.Equals(default(Guid)))
                {
                    CurrentSessionApplicationState.CurrentUser.HasProjects = year == "0" ? false : UserService.CheckPersonHasProjects(CurrentSessionApplicationState.CurrentUser, _surveyRepository.GetYearId(year, DRCOG.Domain.Enums.TimePeriodType.Survey));

                    if (CurrentSessionApplicationState.CurrentUser.HasProjects)
                    {
                        return RedirectToAction("ProjectList", new { @year = year, @dft = "MyProjects" });
                    }
                }
                return RedirectToAction("Dashboard", new { @year = year, @listType = "Sponsor" });
            }
            else CurrentSessionApplicationState.CurrentUser.HasProjects = UserService.CheckPersonHasProjects(CurrentSessionApplicationState.CurrentUser, _surveyRepository.GetYearId(year, DRCOG.Domain.Enums.TimePeriodType.Survey));
            
            var viewModel = _surveyRepository.GetListViewModel();
            //viewModel.Current = 
            //viewModel.RtpSummary = _surveyRepository.GetSummary(year);
            ////reset search model when going back to index.
            //CurrentSessionApplicationState.ProjectSearchModel = null;
            return View("Surveylist", viewModel);
        }


        #endregion

        #region Survey Dashboard Tab /Survey/Dashboard/<year>

        /// <summary>
        /// Display the RTP Dashboard
        /// </summary>
        /// <param name="id"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        //[RoleAuth]
        public ActionResult Dashboard(string year, string listType, bool? showAll)
        {
            LoadSession();

            //if (String.IsNullOrEmpty(year))
            //{
            //    year = _surveyRepository.GetCurrentRtpPlanYear();
            //    return RedirectToAction("Dashboard", new { @year = year });
            //}
            CurrentSessionApplicationState.CurrentSurvey = year;

            CurrentSessionApplicationState.CurrentUser.HasProjects = UserService.CheckPersonHasProjects(CurrentSessionApplicationState.CurrentUser, _surveyRepository.GetYearId(year, DRCOG.Domain.Enums.TimePeriodType.Survey));

            //if (appstate.CurrentUser.profile.PersonID.Equals(default(int)))
            //{
            //    UserService.LoadPerson(appstate.CurrentUser.profile);
            //}
            //Sets the default of the Dashboard type. I will set it to Sponsor.
            var dashboardListType =
                String.IsNullOrEmpty(listType) 
                ? 
                (
                    CurrentSessionApplicationState.CurrentUser.IsInRole("Survey Administrator") || CurrentSessionApplicationState.CurrentUser.IsInRole("Administrator")
                    ? DRCOG.Domain.Enums.SurveyDashboardListType.Sponsor
                    : (Request.IsAuthenticated ? DRCOG.Domain.Enums.SurveyDashboardListType.MyProjects : DRCOG.Domain.Enums.SurveyDashboardListType.Sponsor)
                )
                : (DRCOG.Domain.Enums.SurveyDashboardListType)Enum.Parse(typeof(DRCOG.Domain.Enums.SurveyDashboardListType), listType, true);

            DashboardViewModel viewModel = new DashboardViewModel()
            {
                Year = year
                ,
                ListType = dashboardListType
                ,
                Person = CurrentSessionApplicationState.CurrentUser
            };

            //Handle ShowAll bit
            viewModel.ShowAll = (showAll ?? false);

            // get the view model from the repo
            viewModel = _surveyRepository.GetDashboardViewModel(viewModel /*year, dashboardListType*/);
            

            if (viewModel.Person.HasProjects)
            {
                viewModel.Current.AgencyProjectList = _surveyRepository.GetProjects(this.ValidateSearchData(new SearchModel() { SponsorContactId = CurrentSessionApplicationState.CurrentUser.profile.PersonID, Year = year, ShowAllForAgency = true }, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram)));

                CurrentSessionApplicationState.CurrentUser.SponsoredProjectVersionIds = new List<int>();
                foreach (var project in viewModel.Current.AgencyProjectList)
                {
                    CurrentSessionApplicationState.CurrentUser.SponsoredProjectVersionIds.Add(project.ProjectVersionId);
                }

                int index = viewModel.Current.AgencyProjectList.FindIndex(FindIncompleteUpdateStatus);
                if (index >= 0 && viewModel.Current.AgencyProjectList.Count > 0)
                {
                    viewModel.Current.ShowCertification = false;
                }
                else
                {
                    viewModel.Current.ShowCertification = true;
                    foreach (Project p in viewModel.Current.AgencyProjectList)
                    {
                        p.Funding = _surveyRepository.GetFunding(p.ProjectVersionId, p.TimePeriod);
                    }
                }
            }

            viewModel.Current.SponsorsOrganization = new SponsorOrganization()
            {
                OrganizationId = viewModel.Person.SponsorOrganizationId
                ,
                OrganizationName = viewModel.Person.SponsorOrganizationName
            };

            viewModel.Current.ProjectSponsorsModel = _surveyRepository.GetProjectSponsorsModel(default(int), year);

            // TODO: the following line can/should be set in the repo
            //viewModel.ListType = dashboardListType;

            return View(viewModel);
        }

        #endregion

        #region Project List

        //[RoleAuth]
        //public ActionResult ResetSearchModel(string year)
        //{
        //    LoadSession();
        //    CurrentSessionApplicationState.ProjectSearchModel = null;
        //    return RedirectToAction("ProjectList", new { year = year });
        //}

        /// <summary>
        /// Returns a list of projects associated with this RTP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[RoleAuth]
        public ActionResult ProjectList(string year, string dft, string df, int? page)
        {
            LoadSession();
            //if (String.IsNullOrEmpty(year))
            //{
            //    year = _surveyRepository.GetCurrentRtpPlanYear();
            //    return RedirectToAction("Dashboard", new { @year = year });
            //}
            //Make a ProjectViewModel object from the search criteria
            var projectSearchModel = new SearchModel();
            Survey survey = _surveyRepository.GetSurvey(year);

            //If there is a 'df' dashboard filter, then the Session search criteria are reset.
            if (df != null)
            {
                //Reset Session search criteria
                CurrentSessionApplicationState.ProjectSearchModel = null;

                //Assign dashboard search filter criteria
                projectSearchModel.Year = year;

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
                    case "MyProjects":
                        projectSearchModel.SponsorAgency = df;
                        projectSearchModel.SponsorContactId = CurrentSessionApplicationState.CurrentUser.profile.PersonID;
                        break;
                }

                //Assume from dashboard that we only want active projects.
                //projectSearchModel.VersionStatusId = rtpSummary.IsPending ? (int)RTPVersionStatus.Pending : (int)RTPVersionStatus.Active;
            }
            else
            {
                if (dft != null)
                {
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
                        case "MyProjects":
                            projectSearchModel.SponsorAgency = df;
                            //projectSearchModel.SponsorContactId = appstate.CurrentUser.profile.PersonID;
                            projectSearchModel.ShowMySponsorAgencies = true; 
                            projectSearchModel.ShowAllForAgency = true;
                            projectSearchModel.Profile = CurrentSessionApplicationState.CurrentUser.profile;
                            break;
                    }
                }
                //Check to see if there is a projectSearchModel in Session. If not, then we have nt selected a dashboard or project search tab option.
                if (CurrentSessionApplicationState.ProjectSearchModel != null)
                {
                    //Pull ProjectSearchModel from session and use
                    projectSearchModel = (SearchModel)CurrentSessionApplicationState.ProjectSearchModel;
                }
                else
                {
                    //Create search using RTPYear and Active Version only (default).
                    projectSearchModel.Year = year;
                    //projectSearchModel.VersionStatusId = rtpSummary.IsPending ? (int)RTPVersionStatus.Pending : (int)RTPVersionStatus.Active;
                }
                projectSearchModel.Year = year;
                projectSearchModel.ShowAllForAgency = true;
            }

            //Before passing the ProjectSearchModel, make sure it is validated
            projectSearchModel = this.ValidateSearchData((SearchModel)projectSearchModel, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram));

            //DTS NOTE: We don't fetch the model from the Repo directly because we will have to handle some complex criteria and filtering options
            var viewModel = new ProjectListViewModel();
            viewModel.Current = survey;
            //viewModel.RtpSummary.RtpYear = year;
            viewModel.ProjectList = _surveyRepository.GetProjects(projectSearchModel);
            
            viewModel.ListCriteria = df;
            viewModel.ListType = dft;
            if (viewModel.ProjectList.Count > 1000)
            {
                int originalCount = viewModel.ProjectList.Count;
                viewModel.ProjectList = viewModel.ProjectList.Take(1000).ToList();
                ViewData["ShowMessage"] = "Your results exceeded 1000 records. Please refine your search to narrow your results";
            }
            //viewModel.RestorableProjectList = _surveyRepository.GetRestoreProjectList(_surveyRepository.GetYearId(year));
            //Now save this projectSearchModel (for future searchs)
            CurrentSessionApplicationState.ProjectSearchModel = projectSearchModel;

            viewModel.Person = CurrentSessionApplicationState.CurrentUser;

            viewModel.Current.SponsorsOrganization = new SponsorOrganization()
            {
                OrganizationId = viewModel.Person.SponsorOrganizationId
                ,
                OrganizationName = viewModel.Person.SponsorOrganizationName
            };

            if (viewModel.Person.HasProjects || viewModel.Current.IsAdmin())
            {
                if (viewModel.Current.IsAdmin())
                {
                    viewModel.Current.AgencyProjectList = viewModel.ProjectList;
                }
                else
                {
                    viewModel.Current.AgencyProjectList = _surveyRepository.GetProjects(this.ValidateSearchData(new SearchModel() { ShowMySponsorAgencies = true, Year = year, ShowAllForAgency = true, Profile = CurrentSessionApplicationState.CurrentUser.profile }, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram)));
                }
                int index = viewModel.Current.AgencyProjectList.FindIndex(FindIncompleteUpdateStatus);
                if (index >= 0)
                {
                    viewModel.Current.ShowCertification = false;
                    viewModel.Current.AgencyProjectList = null;
                    // element exists, do what you need
                }
                else
                {
                    viewModel.Current.ShowCertification = true;

                    List<Project> projects = new List<Project>();
                    foreach (Project p in viewModel.Current.AgencyProjectList)
                    {
                        p.Funding = _surveyRepository.GetFunding(p.ProjectVersionId, p.TimePeriod);
                        projects.Add(p);
                        //_surveyRepository.GetSponsorContacts(projects.Add(p));
                    }

                    viewModel.Current.AgencyProjectList = projects;
                    viewModel.Current.AgencySponsorContacts = _surveyRepository.GetContact(new ContactSearch()
                    {
                        ShowAllForAgency = true
                        ,
                        TimePeriodId = projectSearchModel.YearId
                        ,
                        PersonId = CurrentSessionApplicationState.CurrentUser.profile.PersonID
                    });
                }
            }

            viewModel.Current.ProjectSponsorsModel = _surveyRepository.GetProjectSponsorsModel(default(int), year);


            return View(viewModel);
        }

        // Explicit predicate delegate.
        private static bool FindIncompleteUpdateStatus(Project p)
        {
            return p.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited)
                || p.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Carryover)
                || p.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.AwaitingAction);
        }

        //[RoleAuth]
        //public JsonResult GetAvailableRestoreProjects(string plan)
        //{

        //    var result = new List<SelectListItem>();

        //    try
        //    {
        //        var availableProjects = _surveyRepository.GetRestoreProjectList(_surveyRepository.GetYearId(plan));
        //        result.Add(new SelectListItem { Text = "", Value = "" });
        //        availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.RtpYear + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Clear();
        //        result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
        //        return Json(new { response = result });

        //    }
        //    return Json(result);
        //}

        //[RoleAuth]
        //public JsonResult GetAmendableProjects(string plan)
        //{

        //    var result = new List<RtpSummary>();

        //    try
        //    {
        //        result = _surveyRepository.GetAmendableProjects(_surveyRepository.GetYearId(plan)).ToList();
        //        //result.Add(new SelectListItem { Text = "", Value = "" });
        //        //availableProjects.ToList();//.ForEach(x => { result.Add(new SelectListItem { Text = x.RtpYear + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Clear();
        //        //result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
        //        return Json(new { response = result });

        //    }
        //    return Json(result);
        //}

        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult DeleteProjectVersion(int projectVersionId)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = _rtpProjectRepository.DeleteProjectVersion(projectVersionId, Enums.RTPAmendmentStatus.Submitted);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = "Changes could not be stored. An error has been logged."
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        data = result
        //        ,
        //        message = "Project successfully removed."
        //        ,
        //        error = "false"
        //    });

        //}
        
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult Restore(string plan, int id)
        //{
        //    RtpSummary result = null;
        //    try
        //    {
        //        IRestoreStrategy strategy = new RestoreStrategy(this._rtpProjectRepository, id).PickStrategy();
        //        result = (RtpSummary)strategy.Restore(plan);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = "Changes could not be stored. An error has been logged."
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        data = result
        //        //data = new
        //        //{
        //        //    RtpYear = result.RtpYear,
        //        //    Title = result.Title,
        //        //    ProjectVersionId = result.ProjectVersionId,
        //        //    COGID = result.COGID,
        //        //    AmendmentStatus = result.AmendmentStatus,
        //        //    ImprovementType = result.ImprovementType,
        //        //    SponsorAgency = result.SponsorAgency
        //        //}
        //        ,
        //        message = "Project successfully restored to " + plan + "."
        //        ,
        //        error = "false"
        //    });
        //}


        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult Amend(int projectVersionId, int cycleId)
        //{
        //    int result = default(int);
        //    CycleAmendment amendment = new CycleAmendment() { ProjectVersionId = projectVersionId, Id = cycleId };
        //    try
        //    {
        //        IAmendmentStrategy strategy = new AmendmentStrategy(_rtpProjectRepository, amendment).PickStrategy();
        //        result = strategy.Amend();
        //        //result = _rtpProjectRepository.DeleteProjectVersion(projectVersionId, RTPAmendmentStatus.Submitted);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = "Changes could not be stored. An error has been logged."
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        data = result
        //        ,
        //        message = "Project successfully amended."
        //        ,
        //        error = "false"
        //    });

        //}

        //[RoleAuth]
        public PartialViewResult CreatePartial(Survey model)
        {
            LoadSession();

            CreateProjectViewModel viewModel = _surveyRepository.GetCreateProjectViewModel(model);
            return PartialView("~/Views/Survey/Partials/CreatePartial.ascx", viewModel);
        }

        //[RoleAuth]
        public JsonResult CreateProject(string projectName, string facilityName, int timePeriodId, int sponsorOrganizationId, int sponsorContactId, int improvementTypeId, string startAt, string endAt)
        {
            int projectVersionId = default(int);

            try
            {
                projectVersionId = _surveyRepository.CreateProject(projectName, facilityName, timePeriodId, sponsorOrganizationId, sponsorContactId, improvementTypeId, startAt, endAt);

                
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
                message = "Survey project successfully created."
                ,
                error = "false"
                ,
                data = projectVersionId
            });
        }

        #endregion

        #region Project Search

        ///// <summary>
        ///// Get the ProjectSearch view.
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //[RoleAuth]
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult ProjectSearch(string year)
        //{
        //    LoadSession();
        //    var viewModel = new ProjectSearchViewModel();

        //    //Get the current criteria out of session and stuff into the view model
        //    //ApplicationState session = this.GetSession();
        //    viewModel = _surveyRepository.GetProjectSearchViewModel(year,StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram));
        //    viewModel.ProjectSearchModel = this.GetProjectSearchModel();

        //    //Lastly, since ProjectSearchViewModel does not accept RtpYear as a Parameter, we will assign the TipYear here. -DBD
        //    viewModel.RtpSummary.RtpYear = year;

        //    return View(viewModel);
        //}

        //private RTPSearchModel GetProjectSearchModel()
        //{
        //    //This method will check to see if a ProjectSearchModel is already in exsitance in the Session object.
        //    //If so, it will copy it to the current object. If not, then defaults will be returned.

        //    //ToDo: Add a variable for the application, so that each application can specifiy its own defaults?

        //    var result = new RTPSearchModel();
        //    LoadSession();
        //    //Get a reference to session object
        //    //ApplicationState appSession = this.GetSession();

        //    if (CurrentSessionApplicationState.ProjectSearchModel != null)
        //    {
        //        result = (RTPSearchModel)CurrentSessionApplicationState.ProjectSearchModel;
        //    }
        //    else
        //    {
        //        _surveyRepository.SetProjectSearchDefaults(result);
        //        //Just return some general defaults for now
        //        result.AmendmentStatusID = null;
        //        result.COGID = "";
        //        result.ProjectName = "";
        //        result.ProjectType = "";
        //        result.SponsorAgencyID = null;
        //        result.RtpID = "";
        //        //result.RtpYearID = 8;
        //        result.ImprovementTypeID = null;
        //        result.VersionStatusId = (int)Enums.RTPVersionStatus.Active;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Store the criteria and redirect to the ProjectList
        ///// which will then apply the new criteria.
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[RoleAuth]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult ProjectSearch(ProjectSearchViewModel model)
        //{
        //    LoadSession();

        //    //Save search options to session
        //    if (CurrentSessionApplicationState.ProjectSearchModel != null) CurrentSessionApplicationState.ProjectSearchModel = null;
        //    //if (Session["ProjectSearchModel"] != null) Session.Remove("ProjectSearchModel");
        //    CurrentSessionApplicationState.ProjectSearchModel = model.ProjectSearchModel;
        //    //Session.Add("ProjectSearchModel", model.ProjectSearchModel);

        //    //Redirect to the project list. Determine RTP Year (text) for redirect from appSession
        //    return new RedirectToRouteResult(
        //                      new RouteValueDictionary {                              
        //                      { "controller", "RTP" },
        //                      { "action", "ProjectList" },
        //                      { "year", CurrentSessionApplicationState.CurrentRTP }                             
        //                });
        //}

        
        //public JsonResult GetPlanScenarios(int planYearId)
        //{
        //    var result = new List<SelectListItem>();

        //    try
        //    {
        //        var planScenarios = _surveyRepository.GetPlanScenarios(planYearId);
        //        result.Add(new SelectListItem { Text = "", Value = "" });
        //        planScenarios.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Clear();
        //        result.Add(new SelectListItem { Text = "No Scenarios found", Value = "0" });
        //        return Json(new { response = result });

        //    }
        //    return Json(result);
        //}

        //public JsonResult GetPlanScenariosForCurrentCycle(int planYearId)
        //{
        //    var result = new List<SelectListItem>();

        //    try
        //    {
        //        var planScenarios = _surveyRepository.GetPlanScenariosForCurrentCycle(planYearId);
        //        result.Add(new SelectListItem { Text = "", Value = "" });
        //        planScenarios.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Clear();
        //        result.Add(new SelectListItem { Text = "No Scenarios found", Value = "0" });
        //        return Json(new { response = result });

        //    }
        //    return Json(result);
        //}
        


        #endregion

        #region RTP Amendments Tab

        //[RoleAuth]
        //public ActionResult Amendments(string year)
        //{
        //    var viewModel = new RtpBaseViewModel();
        //    viewModel.RtpSummary.RtpYear = year;

        //    return View(viewModel);
        //}

        //#endregion

        //#region RTP Status /RTP/<year>/Status/

        ///// <summary>
        ///// Display the status for a RTP
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public ActionResult Status(string year)
        //{
        //    // get the view model from the repo
        //    var viewModel = _surveyRepository.GetRtpStatusViewModel(year);
        //    viewModel.RtpSummary = _surveyRepository.GetSummary(year);
        //    return View("Status", viewModel);
        //}

        ///// <summary>
        ///// Update the Status for a RTP
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public ActionResult UpdateStatus(StatusViewModel viewModel)
        //{

        //    RtpStatusModel model = new RtpStatusModel();
        //    //StatusViewModel model = new StatusViewModel();
        //    UpdateModel(model);

        //    if (!ModelState.IsValid)
        //    {
        //        viewModel.RtpStatus = model;
        //        return View("Status", viewModel);
        //    }

        //    //Send update to repo
        //    try
        //    {
        //        _surveyRepository.UpdateRtpStatus(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { message = "Changes could not be stored. An error has been logged." });
        //    }
        //    return Json(new { message = "Changes successfully saved." });
        //}

        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult UpdateTimePeriodStatusId(int timePeriodId, int statusId)
        //{
        //    string error = String.Empty;
        //    try
        //    {
        //        error = _surveyRepository.UpdateTimePeriodStatusId(timePeriodId, statusId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Project Financial Record Detail successfully added."
        //        ,
        //        error = "false"
        //    });
        //}

        ///// <summary>
        ///// Add a cycle to the plan
        ///// </summary>
        ///// <param name="planId"></param>
        ///// <param name="cycleId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult AddCycle(string plan, int cycleId)
        //{
        //    var jsr = new JsonServerResponse();
        //    jsr.Error = _surveyRepository.AddCycleToTimePeriod(plan, cycleId);
        //    return Json(jsr);
        //}

        ///// <summary>
        ///// Remove a cycle
        ///// </summary>
        ///// <param name="cycleId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult DropCycle(int cycleId)
        //{
        //    LoadSession();

        //    var jsr = new JsonServerResponse();
        //    jsr.Error = _surveyRepository.RemoveCycleFromTimePeriod(cycleId);
        //    if (!jsr.Error.Equals(""))
        //    {
        //        jsr.Error = "Cycle is in use in the current Plan. It can not be removed.";
        //    }
        //    return Json(jsr);
        //}

        ///// <summary>
        ///// Create a Cycle
        ///// </summary>
        ///// <param name="cycle"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult CreateCycle(string cycle)
        //{
        //    string error = String.Empty;
        //    try
        //    {
        //        error = _surveyRepository.CreateCycle(cycle);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Project Financial Record Detail successfully added."
        //        ,
        //        error = "false"
        //    });
        //}

        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult UpdateCycleName(int cycleId, string cycle)
        //{
        //    string error = String.Empty;
        //    try
        //    {
        //        error = _surveyRepository.UpdateCycleName(cycleId, cycle);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Successfully updated."
        //        ,
        //        error = "false"
        //    });
        //}

        //public JsonResult GetSponsorOrganizations(string plan)
        //{
        //    var result = new List<SelectListItem>();
        //    var sponsorOrganizations = _surveyRepository.GetCurrentTimePeriodSponsorAgencies(plan, Enums.ApplicationState.RTP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
        //    sponsorOrganizations.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
        //    return new JsonResult
        //    {
        //        Data = result
        //    };
        //}

        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult SetActiveCycle(int cycleId, int timePeriodId)
        //{
        //    string error = String.Empty;
        //    try
        //    {
        //        error = _surveyRepository.SetActiveCycle(cycleId, timePeriodId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Project Financial Record Detail successfully added."
        //        ,
        //        error = "false"
        //    });
        //}

        //public JsonResult GetPlanAvailableProjects(int planId, int cycleId)
        //{

        //    var result = new List<SelectListItem>();

        //    try
        //    {
        //        var availableProjects = _surveyRepository.GetPlanAvailableProjects(planId, cycleId);
        //        result.Add(new SelectListItem { Text = "", Value = "" });
        //        availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Clear();
        //        result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
        //        return Json(new { response = result });

        //    }
        //    return Json(result);
        //}

        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult UpdateCycleSort(string cycles)
        //{

        //    string error = String.Empty;
        //    try
        //    {
        //        _surveyRepository.UpdateTimePeriodCycleOrder(cycles);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            message = String.IsNullOrEmpty(error) ? "Changes could not be stored." : error
        //            ,
        //            error = "true"
        //            ,
        //            exceptionMessage = ex.Message
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Project Financial Record Detail successfully added."
        //        ,
        //        error = "false"
        //    });
        //} 

        ////[RoleAuth]
        ////public ActionResult Amend(StatusViewModel model)
        ////{
        ////    var amendment = model.Cycle;
        ////    //this.Amend(amendment);
        ////    return View();
        ////    //return RedirectToAction("Funding", new { controller = "Project", id = projectVersionId, message = "Amendment processed successfully." });
        ////}

        ////[RoleAuth]
        ////public ActionResult Amend(Cycle cycle)
        ////{
        ////    foreach (CycleAmendment amendment in cycle.Projects)
        ////    {
        ////        IAmendmentStrategy strategy = new AmendmentStrategy(_rtpProjectRepository, amendment).PickStrategy();
        ////        //strategy.Amend();
        ////    }
        ////    return View();
        ////}

        /////// <summary>
        /////// Get the Reports View
        /////// </summary>
        /////// <param name="year"></param>
        /////// <returns></returns>
        ////[RoleAuth]
        ////public ActionResult Reports(string year)
        ////{
        ////    //Create the ViewModel
        ////    ReportsViewModel model = new ReportsViewModel();
        ////    model.TipSummary = _surveyRepository.GetTIPSummary(year);
        ////    return View("reports", model);
        ////}

        #endregion

        #region RTP Eligible Agencies Tab

        ///// <summary>
        ///// Display the eligible agencies lists for a TIP
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //[RoleAuth]
        //public ActionResult Agencies(string year)
        //{

        //    //get the mode from the repo
        //    SponsorsViewModel model = _surveyRepository.GetSponsorsViewModel(year);

        //    return View(model);
        //}

        ///// <summary>
        ///// Callback to see if an agency can be dropped from the tip
        ///// </summary>
        ///// <param name="year"></param>
        ///// <param name="agencyId"></param>
        ///// <returns></returns>
        ////[RoleAuth]
        ////public ActionResult CheckAgency(string year, int agencyId)
        ////{
        ////    if (_tipRepository.CanAgencyBeDropped(year, agencyId))
        ////    {
        ////        return Json(true);
        ////    }
        ////    else
        ////    {
        ////        return Json(false);
        ////    }
        ////}

        ///// <summary>
        ///// Update the eligible agencies associated with this RTP
        ///// </summary>
        ///// <param name="tipYear"></param>
        ///// <param name="added"></param>
        ///// <param name="removed"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult UpdateAgencies(string plan, List<int> added, List<int> removed)
        //{
        //    if (added == null)
        //    {
        //        added = new List<int>();
        //    }
        //    if (removed == null)
        //    {
        //        removed = new List<int>();
        //    }
        //    //Send the two lists of ID's to the Repo
        //    try
        //    {
        //        _surveyRepository.UpdateEligibleAgencies(plan, added, removed);
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Data = true;
        //        return Json(jsr);
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Error = ex.Message;
        //        return Json(jsr);
        //    }
        //}


        ///// <summary>
        ///// Remove an agency from the eligible agencies list
        ///// if the agency does not sponsor any projects in the
        ///// tip
        ///// </summary>
        ///// <param name="plan"></param>
        ///// <param name="agencyId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[RoleAuth(Roles = "Administrator, RTP Administrator")]
        //public JsonResult DropEligibleAgency(string plan, int agencyId)
        //{
        //    LoadSession();

        //    var jsr = new JsonServerResponse();
        //    jsr.Error = _surveyRepository.DropAgencyFromTimePeriod(plan, agencyId, appstate.State.CurrentProgram);
        //    if (!jsr.Error.Equals(""))
        //    {
        //        jsr.Error = "That agency sponsors projects in the current Plan. They can not be removed.";
        //    }
        //    return Json(jsr);
        //}

        #endregion

        #region RTP FundingList Tab

        //public ActionResult FundingList(string year, int? page)
        //{

        //    var viewModel = new FundingSourceListViewModel();
        //    viewModel.RtpSummary = _surveyRepository.GetSummary(year);
        //    viewModel.FundingSources = _surveyRepository.GetFundingSources(year); //.AsPagination(page.GetValueOrDefault(1), 10);                               
        //    return View("FundingList", viewModel);
        //}

        #endregion 

        #region Project Controller Members

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult SetSurveyStatus(Project project)
        {
            try
            {
                if (project.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited))
                {

                } else _surveyRepository.SetSurveyStatus(project);
                //_surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                return Json(new { message = "Changes could not be stored. An error has been logged."
                    , error = "true"
                    , exceptionMessage = ex.Message });
            }
            return Json(new { message = "Changes were successful."
                    , error = "false" });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public ActionResult Edit(int projectVersionId, int previousVersionId)
        {
            Instance version = new Instance()
            {
                ProjectVersionId = projectVersionId
                ,
                PreviousVersionId = previousVersionId
                ,
                UpdateStatusId = (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited
            };
            IAmendmentStrategy strategy = new AmendmentStrategy(_surveyRepository, version).PickStrategy();
            projectVersionId = strategy.Amend();

            return RedirectToAction("Info", new { controller = "Survey", id = projectVersionId, message = "Amendment processed successfully." });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        //public JsonResult CarryOver(int projectVersionId)
        //{
        //    try
        //    {
        //        Instance version = new Instance()
        //        {
        //            ProjectVersionId = projectVersionId
        //            ,
        //            UpdateStatusId = (int)Enums.SurveyUpdateStatus.AwaitingAction
        //        };
        //        IAmendmentStrategy strategy = new AmendmentStrategy(_surveyRepository, version).PickStrategy();
        //        projectVersionId = strategy.Amend();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { message = "Changes could not be stored. An error has been logged."
        //            , error = "true"
        //            , exceptionMessage = ex.Message });
        //    }
        //    return Json(new { message = "Changes were successful."
        //            , error = "false" });

        //    //return RedirectToAction("Info", new { controller = "Survey", id = projectVersionId, message = "Amendment processed successfully." });
        //}

        #region Project Info

        public ActionResult Info(string year, int id)
        {
            this.LoadSession();
            var viewModel = _surveyRepository.GetProjectInfoViewModel(id, year);

            viewModel.Project.IsSponsorContact = viewModel.Project.IsContributor(CurrentSessionApplicationState.CurrentUser.profile.PersonID);
            
            //viewModel.Person = appstate.CurrentUser.profile;
            return View(viewModel);
        }

        /// <summary>
        /// Update the General Information for a project
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public ActionResult UpdateInfo(InfoViewModel viewModel)
        {
            LoadSession();

            //if (appstate.CurrentUser.LastProjectVersionId == viewModel.Project.ProjectVersionId)
            //{
                int projectVersionId = viewModel.Project.ProjectVersionId;
                string year = viewModel.Current.Name;
                //Get the model from the database
                Project model = _surveyRepository.GetProjectInfo(projectVersionId, year);
                //Update it - UpdateModel was being wonky so it's a left/right-copy -DB    -- Did he say 'wonky'? Is that a word? -DBD
                model.AdministrativeLevelId = viewModel.Project.AdministrativeLevelId;
                model.DRCOGNotes = viewModel.Project.DRCOGNotes;
                model.ImprovementTypeId = viewModel.Project.ImprovementTypeId;
                model.ProjectId = viewModel.Project.ProjectId;
                model.ProjectName = viewModel.Project.ProjectName;
                model.ProjectVersionId = viewModel.Project.ProjectVersionId;
                model.SponsorContactId = viewModel.Project.SponsorContactId;
                model.SponsorId = (int)viewModel.ProjectSponsorsModel.PrimarySponsor.OrganizationId;
                model.SponsorNotes = viewModel.Project.SponsorNotes;
                model.TimePeriod = viewModel.Current.Name;
                model.TransportationTypeId = viewModel.Project.TransportationTypeId;
                model.UpdateStatusId = viewModel.Project.UpdateStatusId;
                model.Funding = viewModel.Project.Funding;

                ModelState.Remove("Project.SponsorContactId");

                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();
                    string html_ul_errors = "<ul>";

                    foreach (string error in errorList)
                    {
                        html_ul_errors += "<li>" + error + "</li>";
                    }
                    html_ul_errors += "</ul>";
                    return Json(new { error = "Changes could not be stored. An error has been logged." + "<br />" + html_ul_errors });
                }

                //Send update to repo
                try
                {
                    _surveyRepository.UpdateProjectInfo(model);
                }
                catch (Exception ex)
                {
                    //this.Logger.LogMethodError("ProjectController", "UpdateInfo", "TipProjectInfoViewModel", ex);
                    return Json(new { error = "Changes could not be stored. An error has been logged." });
                }
                return Json(new { message = "Changes successfully saved." });
            //}
            //return Json(new { message = "You are not authorized to modifiy this page." });

        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator")]
        public JsonResult UpdateAvailableSponsorContacts(int id)
        {
            var result = new List<SelectListItem>();
            //result.Add(new SelectListItem { Value = "1", Text = "Nick Kirkes" });
            var contacts = _surveyRepository.GetSponsorContacts(id);
            contacts.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            return new JsonResult
            {
                Data = result
            };

        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult AddFundingSource(int fundingResourceId, int projectVersionId)
        {
            var fundingSource = new FundingSource() { Id = fundingResourceId };
            try
            {
                _surveyRepository.AddFundingSource(fundingSource, projectVersionId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Funding Source successfully Added." });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult DeleteFundingSource(int fundingResourceId, int projectVersionId)
        {
            var fundingSource = new FundingSource() { Id = fundingResourceId };
            try
            {
                _surveyRepository.DeleteFundingSource(fundingSource, projectVersionId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Funding Source successfully removed." });
        }

        #endregion

        #region Scope

        /// <summary>
        /// Display the Scope for a project
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns></returns>
        public ActionResult Scope(string year, int id)
        {
            this.LoadSession();
            var viewModel = _surveyRepository.GetScopeViewModel(id, year);
            //viewModel.ProjectSummary.IsEditable = false;     
            viewModel.Project.IsSponsorContact = viewModel.Project.IsContributor(CurrentSessionApplicationState.CurrentUser.profile.PersonID);
            //viewModel.Project.IsSponsorContact = LoadSession(viewModel.Project);

            return View(viewModel);
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        [HttpPost]
        public ActionResult UpdateScope(ScopeViewModel viewModel)
        {
            int projectVersionId = viewModel.Scope.ProjectVersionId;
            string year = viewModel.Current.Name;
            //Get the model from the database
            ScopeModel model = _surveyRepository.GetScopeModel(projectVersionId, year);
            Project project = _surveyRepository.GetProjectBasics(projectVersionId);
            //Update it
            model.ProjectDescription = viewModel.Scope.ProjectDescription;
            model.ProjectVersionId = viewModel.Scope.ProjectVersionId;
            model.OpenToPublicYear = viewModel.Scope.OpenToPublicYear;
            model.BeginConstructionYear = viewModel.Scope.BeginConstructionYear;

            if (!ModelState.IsValid)
            {
                return View("Scope", viewModel);
            }

            //Send update to repo
            try
            {
                _surveyRepository.UpdateProjectScope(model, project);
                
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
                data = false
                ,
                message = "Changes successfully saved."
                ,
                error = "false"
            });
        }

        public JsonResult GetSegmentDetails(int segmentId)
        {
            SegmentModel model;
            try
            {
                model = _surveyRepository.GetSegmentDetails(segmentId);
                XMLService xml = new XMLService(_surveyRepository);
                model._LRS = xml.LoadRecord((int)SchemeName.LRSProjects, model.LRSObjectID);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Segment not found." });
            }
            return Json(model);
        }

        public ActionResult GetDetailsOverview(jQueryDataTableParamModelExtension param)
        {
            var allLists = _surveyRepository.GetDetailsOverview(param.sTimePeriodId);

            IEnumerable<SurveyOverview> filteredLists;
            if (!string.IsNullOrWhiteSpace(param.sSearch))
            {
                filteredLists = allLists
                    .Where(c => c.ProjectName.Contains(param.sSearch)
                        || c.COGID.Contains(param.sSearch)
                    );
            }
            else
            {
                filteredLists = allLists;
            }

            var displayedLists = filteredLists
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from c in displayedLists
                         select new[] { c.ProjectName, c.OrganizationName, c.COGID, c.ImprovementType, c.Network, c.OpenYear, c.FacilityName, c.StartAt, c.EndAt, c.LanesBase.ToString(), c.LanesFuture.ToString(), c.FacilityType, c.ModelingCheck.ToString(), c.LRSRouteName, c.LRSBeginMeasure, c.LRSEndMeasure };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allLists.Count(),
                iTotalDisplayRecords = filteredLists.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
            
        }

        //[RoleAuth]
        public ActionResult DownloadModelerExtract(int timePeriodId)
        {
            GridView grid = new GridView();
            grid.DataSource = _surveyRepository.GetModelerExtractResults(timePeriodId);
            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=SurveyModelerExtract.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

            return Json(new
            {
                message = "Extract success"
            }, JsonRequestBehavior.AllowGet);
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult AddSegment(SegmentModel model)
        {
            int segmentId = 0;

            try
            {
                segmentId = _surveyRepository.AddSegment(model);
                if (segmentId == 0)
                    throw new Exception("Returned 0 on projectVersionId" + model.ProjectVersionId);

            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully added.", segmentId = segmentId });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateProjectUpdateStatusBySegment(int segmentId)
        {
            try
            {
                var project = _surveyRepository.GetProjectBasicsBySegment(segmentId);
                _surveyRepository.CheckUpdateStatusId(project);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Update Status was not changes successfully. Please go to General Info and change the status to edited." });
            }
            return Json(new { message = "Segment successfully added.", segmentId = segmentId });
        }

        

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult DeleteSegment(int segmentId)
        {
            try
            {
                _surveyRepository.DeleteSegment(segmentId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully removed." });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateSegment(SegmentModel model)
        {
            try
            {
                XMLService xml = new XMLService(_surveyRepository);
                if (!String.IsNullOrEmpty(model.LRSRecord))
                {
                    
                    string data = xml.GenerateXml(xml.GetScheme((int)SchemeName.LRSProjects), new LRSRecord() { Columns = model.LRSRecord.ToDictionary(',') });
                    
                    model.LRSxml = data;
                }
                else model.LRSxml = xml.GenerateXml(null, null);

                _surveyRepository.UpdateSegment(model);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateSegmentSummary(SegmentModel model)
        {
            try
            {
                _surveyRepository.UpdateSegmentSummary(model);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        

        #endregion

        #region Location

        /// <summary>
        /// Display the Location for a project
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns></returns>
        public ActionResult Location(string year, int id)
        {
            this.LoadSession();
            var viewModel = _surveyRepository.GetProjectLocationViewModel(id, year);
            viewModel.Project.IsSponsorContact = viewModel.Project.IsContributor(CurrentSessionApplicationState.CurrentUser.profile.PersonID);
            //viewModel.Project.IsSponsorContact = LoadSession(viewModel.Project);
            return View(viewModel);
        }

        /// <summary>
        /// Update the Location information from the /Location view
        /// </summary>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        [HttpPost]
        public ActionResult UpdateLocation()
        {
            //Manually parse up the form b/c of the muni & county split stuff
            int projectVersionId = Convert.ToInt32(Request.Form["ProjectVersionId"]);
            string year = Request.Form["Year"];
            //Get the existing model from the datagbase
            LocationModel model = _surveyRepository.GetProjectLocationModel(projectVersionId, year);
            //Update values
            model.Limits = Request.Form["Limits"];
            model.FacilityName = Request.Form["FacilityName"];
            int testOut;
            model.RouteId = Int32.TryParse(Request.Form["RouteId"], out testOut) ? Int32.Parse(Request.Form["RouteId"]) : 0;

            //parse out the county & muni shares stuff... 
            Dictionary<int, CountyShareModel> countyShares = ExtractCountyShares(Request.Form);
            Dictionary<int, MunicipalityShareModel> muniShares = ExtractMuniShares(Request.Form);

            //Send updates to repo
            try
            {
                _surveyRepository.UpdateProjectLocationModel(model, projectVersionId);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
                //Update the county shares
                foreach (CountyShareModel m in countyShares.Values)
                {
                    _surveyRepository.UpdateCountyShare(m);
                }
                //Update the muni shares
                foreach (MunicipalityShareModel m in muniShares.Values)
                {
                    _surveyRepository.UpdateMunicipalityShare(m);
                }
                //Ok, we're good.
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdateLocation", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Changes successfully saved." });
        }

        /// <summary>
        /// Add a county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult AddCountyShare(int projectId, int countyId, double share, bool isPrimary, int projectVersionId)
        {
            CountyShareModel model = new CountyShareModel();
            model.ProjectId = projectId;
            model.CountyId = countyId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            
            try
            {
                _surveyRepository.AddCountyShare(model);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "County successfully added." });

        }

        /// <summary>
        /// Update a county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateCountyShare(int projectId, int countyId, double share, bool isPrimary, int projectVersionId)
        {
            CountyShareModel model = new CountyShareModel();
            model.ProjectId = projectId;
            model.CountyId = countyId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _surveyRepository.UpdateCountyShare(model);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "County successfully updated." });

        }

        /// <summary>
        /// Remove a county share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult RemoveCountyShare(int projectId, int countyId, int projectVersionId)
        {
            try
            {
                _surveyRepository.DropCountyShare(projectId, countyId);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "RemoveCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Record successfully dropped." });
        }

        /// <summary>
        /// Add a municipality share to the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult AddMuniShare(int projectId, int muniId, double share, bool isPrimary, int projectVersionId)
        {
            MunicipalityShareModel model = new MunicipalityShareModel();
            model.ProjectId = projectId;
            model.MunicipalityId = muniId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _surveyRepository.AddMunicipalityShare(model);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Municipality successfully added." });
        }

        /// <summary>
        /// Add a municipality share to the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateMuniShare(int projectId, int muniId, double share, bool isPrimary, int projectVersionId)
        {
            MunicipalityShareModel model = new MunicipalityShareModel();
            model.ProjectId = projectId;
            model.MunicipalityId = muniId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _surveyRepository.UpdateMunicipalityShare(model);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Municipality successfully updated." });
        }

        /// <summary>
        /// Drop a municpality share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <returns></returns>
        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult RemoveMuniShare(int projectId, int muniId, int projectVersionId)
        {
            try
            {
                _surveyRepository.DropMunicipalityShare(projectId, muniId);
                _surveyRepository.CheckUpdateStatusId(_surveyRepository.GetProjectBasics(projectVersionId));
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "RemoveMuniShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Record successfully dropped." });
        }

        #endregion

        #region Funding

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateProjectUpdateStatus(int projectVersionId)
        {
            try
            {
                var project = _surveyRepository.GetProjectBasics(projectVersionId);
                _surveyRepository.CheckUpdateStatusId(project);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Update Status was not changes successfully. Please go to General Info and change the status to edited." });
            }
            return Json(new { message = "Update Status successfully updated." });
        }

        public ActionResult Funding(string year, int id)
        {
            this.LoadSession();
            var viewModel = _surveyRepository.GetFundingViewModel(id, year);
            viewModel.Project.IsSponsorContact = viewModel.Project.IsContributor(CurrentSessionApplicationState.CurrentUser.profile.PersonID);
            //viewModel.Project.IsSponsorContact = LoadSession(viewModel.Project);
            return View(viewModel);
        }

        //[RoleAuth(Roles = "Administrator, Survey Administrator, Sponsor")]
        public JsonResult UpdateFinancialRecord(Project model)
        {
            try
            {
                _surveyRepository.UpdateFinancialRecord(model);
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
                message = "Project Financial Record successfully updated."
                ,
                error = "false"
            });
        }

        

        #endregion

        #endregion

        //[RoleAuth]
        public JsonResult SendPrintVerification(string sponsorName)
        {
            //DRCOGConfig config = DRCOGConfig.GetConfig();
            //string emailConfirmationPage = config.EmailConfirmationPage;

            // who to send it too?

            string emailTo = "ltilong@drcog.org";
            string emailSubject = "[TRIPS-Survey] " + sponsorName + " Certification Printed";
            string emailBody = "";

            //Make body of email
            emailBody += (new CultureInfo("en-US", false).TextInfo).ToTitleCase(sponsorName.ToLower()) +
                " has printed it's Transportation Improvement Survey Certification letter." +
                " The Certification letter will be mailed once it has be signed.";

            IEmailService mail = new EmailService()
            {
                Body = emailBody,
                Subject = emailSubject,
                To = emailTo
            };
            try
            {
                mail.Send();
            }
            catch (Exception exc)
            {
                Exception exci = new Exception("Re/SendVerificationEmail; TO:" + emailTo, exc);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exci);

                return Json(new
                {
                    message = "E-mail notification was not successful. An error has been logged."
                    ,
                    error = "true"
                    ,
                    exceptionMessage = exc.Message
                });
            }
            return Json(new
            {
                message = "Changes were successful."
                ,
                error = "false"
            });
        }

        #region PRIVATE HELPERS

        private SearchModel ValidateSearchData(SearchModel projectSearchModel, string currentProgram)
        {
            //Check completeness of Year
            if ((projectSearchModel.YearId.Equals(default(int)) && (!String.IsNullOrEmpty(projectSearchModel.Year))))
            {
                //Lookup the YearID
                projectSearchModel.YearId = _surveyRepository.GetYearId(projectSearchModel.Year, DRCOG.Domain.Enums.TimePeriodType.Survey);
            }

            if ((!projectSearchModel.YearId.Equals(default(int)) && (String.IsNullOrEmpty(projectSearchModel.Year))))
            {
                //Lookup the Year
                projectSearchModel.Year = _surveyRepository.GetYear((int)projectSearchModel.YearId);
            }

            //if(!projectSearchModel.SponsorContactId.Equals(default(int)))
            //{
            //    projectSearchModel.SponsorAgencyID = _surveyRepository.GetSpon
            //}

            //Check completeness of SponsorAgency
            if ((projectSearchModel.SponsorAgencyID == null) && (projectSearchModel.SponsorAgency != null))
            {
                //Lookup the SponsorAgencyID
                projectSearchModel.SponsorAgencyID = _surveyRepository.GetSponsorAgencyID(projectSearchModel.SponsorAgency);
            }

            if ((projectSearchModel.SponsorAgencyID != null) && (projectSearchModel.SponsorAgency == null))
            {
                //Lookup the SponsorAgency
                projectSearchModel.SponsorAgency = _surveyRepository.GetSponsorAgency(projectSearchModel.SponsorAgencyID);
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
                    statusType = "Survey Amendment Status"; // If something goes wrong, assume Survey
                    break;
            }

            //Check completeness of AmendmentStatus
            if ((projectSearchModel.AmendmentStatusID == null) && (projectSearchModel.AmendmentStatus != null))
            {
                //Lookup the AmendmentStatusID
                projectSearchModel.AmendmentStatusID = _surveyRepository.GetStatusID(projectSearchModel.AmendmentStatus, statusType);
            }

            if ((projectSearchModel.AmendmentStatusID != null) && (projectSearchModel.AmendmentStatus == null))
            {
                //Lookup the AmendmentStatus
                projectSearchModel.AmendmentStatus = _surveyRepository.GetStatus(projectSearchModel.AmendmentStatusID, statusType);
            }

            //Check completeness of ImprovementType
            if ((projectSearchModel.ImprovementTypeID == null) && (projectSearchModel.ImprovementType != null))
            {
                //Lookup the ImprovementTypeID
                projectSearchModel.ImprovementTypeID = _surveyRepository.GetImprovementTypeID(projectSearchModel.ImprovementType);
            }

            if ((projectSearchModel.ImprovementTypeID != null) && (projectSearchModel.ImprovementType == null))
            {
                //Lookup the ImprovementType
                projectSearchModel.ImprovementType = _surveyRepository.GetImprovementType(projectSearchModel.ImprovementTypeID);
            }

            //Check completeness of ProjectType
            if ((projectSearchModel.ProjectTypeID == null) && (projectSearchModel.ProjectType != null))
            {
                //Lookup the ProjectTypeID
                projectSearchModel.ProjectTypeID = _surveyRepository.GetProjectTypeID(projectSearchModel.ProjectType);
            }

            if ((projectSearchModel.ProjectTypeID != null) && (projectSearchModel.ProjectType == null))
            {
                //Lookup the ProjectType
                projectSearchModel.ProjectType = _surveyRepository.GetProjectType(projectSearchModel.ProjectTypeID);
            }

            return projectSearchModel;
        }

        #endregion
    }
}