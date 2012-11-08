#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR	        REMARKS
 * 07/15/2009	DBouwman        1. Initial Creation (DTS). 
 * 02/01/2010   DDavidson       2. Multiple improvements.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DRCOG.Domain;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIP;
using DRCOG.Domain.ViewModels.TIPProject;
using Trips4;
using DTS.Web.MVC;
//using MvcContrib.Pagination;
using Trips4.Utilities.ApplicationState;
using DRCOG.Domain.Helpers;
using DRCOG.Domain.Models.TIPProject;
using Trips4.Configuration;
using DRCOG.TIP.Services.TIP;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services.RestoreStrategy.TIP;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using DRCOG.Common.Util;
using DRCOG.Common.Web.MvcSupport.Attributes;
using DRCOG.Domain.Models.TIP;
using System.Drawing;
using OfficeOpenXml;

namespace Trips4.Controllers
{
    [Trips4.Filters.SessionAuthorizeAttribute]
    public class TipController : ControllerBase
    {
        private readonly ITipRepository _tipRepository;
        private readonly IProjectRepository _projectRepository;
        private Trips4.Data.TripsRepository TripsRepository { get; set; }

        public TipController(ITipRepository tipRepository, IProjectRepository projectRepository, ITripsUserRepository userRepository,
            Trips4.Data.TripsRepository trepo)
            : base("TipController", userRepository)
        {
            _tipRepository = tipRepository;
            _projectRepository = projectRepository;
            TripsRepository = trepo;
        }

        public ActionResult Boom()
        {
            throw new Exception("Danger Will Robinson!");
        }

        private void LoadSession()
        {
            base.LoadSession(Enums.ApplicationState.TIP);
        }

        public JsonResult GetImprovementTypeMatch(int id)
        {
            return base.GetImprovementTypeMatch(id, _tipRepository);
        }

        public JsonResult GetProjectTypeMatch(int id)
        {
            return base.GetProjectTypeMatch(id, _tipRepository);
        }

        public JsonResult GetFundingIncrements(int tipYearId)
        {
            var result = TripsRepository.GetFundingIncrements(tipYearId);
            return Json(new { data = result });
        }


        #region TIP List /TIP

        /// <summary>
        /// Returns a list of the current TIPs
        /// </summary>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Index(string year)
        {
            LoadSession();

            //Save the program selected
            //TIPApplicationState appSession = (TIPApplicationState)Session[DRCOGApp.SessionIdentifier];
            //appstate.State.CurrentProgram = Enums.ApplicationState.TIP;//"Transportation Improvement Plan";
            if (String.IsNullOrEmpty(year) || year == "0")
            {
                year = _tipRepository.GetCurrentTimePeriod();
                //return RedirectToAction("Dashboard", new { @year = year });
            }
            return RedirectToAction("Dashboard", new { @year = year });
            //var viewModel = _tipRepository.GetTipListViewModel();
            //viewModel.TipSummary = _tipRepository.GetTIPSummary(year);

            //return View("Dashboard", viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        [HttpPost]
        public JsonResult CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId, int amendmentTypeId)
        {
            JsonServerResponse jsr = new JsonServerResponse();
            int projectVersionId = 0;
            try
            {
                projectVersionId = _tipRepository.CreateProject(projectName, facilityName, tipYear, sponsorOrganizationId, amendmentTypeId);
            }
            catch (Exception ex)
            {
                jsr.Error = "An error occured creating the TIP";
                return Json(jsr);
            }
            return Json(projectVersionId);
        }


        public JsonResult GetSponsorOrganizations()
        {
            var result = new List<SelectListItem>();
            var sponsorOrganizations = _tipRepository.GetAvailableSponsors();
            sponsorOrganizations.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // for debugging only.
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Denied")]
        public JsonResult GetSponsorOrganizationsUnauth()
        {
            return Json("WTF?", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Expire the session. Used for debugging session handling.
        /// </summary>
        /// <returns></returns>
        public JsonResult ExpireSession()
        {
            Session.Abandon();
            return Json("Session is now defunct.", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create a new TIP
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        [HttpPost]
        public JsonResult CreateTip(int startYear, int endYear, int offset)
        {
            JsonServerResponse jsr = new JsonServerResponse();
            string newTipYear = startYear.ToString() + "-" + endYear.ToString();
            try
            {
                _tipRepository.CreateTip(newTipYear, offset);
                jsr.Data = true;
            }
            catch (Exception ex)
            {
                jsr.Error = "An error occured creating the TIP";
                //_logService.Warn("Create new TIP failed for TipYear='" + newTipYear + "'");
            }

            return Json(jsr);
        }


        public IDictionary<int, string> GetRestoreYears(string tipYearDestination)
        {
            var result = new Dictionary<int, string>();
            var restoreYears = _tipRepository.GetAvailableTipYears().Where(x => !x.Value.Equals(tipYearDestination));
            restoreYears.ToList().ForEach(x => result.Add(x.Key, x.Value));
            return result;
        }

        public JsonResult GetRestoreYearsJSON(string tipYearDestination)
        {
            var result = new List<SelectListItem>();

            GetRestoreYears(tipYearDestination).ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentTimePeriodSponsorAgencies(string year)
        {
            var result = new List<SelectListItem>();
            var sponsors = _tipRepository.GetCurrentTimePeriodSponsorAgencies(year, Enums.ApplicationState.TIP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            sponsors.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region TIP Dashboard Tab /TIP/Dashboard/<tipyear>

        /// <summary>
        /// Display the TIP Dashboard
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="listType"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Dashboard(string year, string listType)
        {
            LoadSession();
            //#region Log callback
            //this.Logger.LogCallbackInfo(this.ControllerName, "Dashboard", "TIP Dashboard page requested.");
            //#endregion

            //Save the TipYear selected
            //TIPApplicationState appSession = (TIPApplicationState)Session[DRCOGApp.SessionIdentifier];
            CurrentSessionApplicationState.CurrentTIP = year;

            //Sets the default of the Dashboard type. I will set it to Sponsor. -DBD
            var dashboardListType =
                String.IsNullOrEmpty(listType) ? Enums.TIPDashboardListType.Sponsor : (Enums.TIPDashboardListType)Enum.Parse(typeof(Enums.TIPDashboardListType), listType, true);

            // get the view model from the repo
            var viewModel = _tipRepository.GetTipDashboardViewModel(year, dashboardListType);

            // TODO: the following line can/should be set in the repo
            viewModel.ListType = dashboardListType;
            viewModel.TipSummary = _tipRepository.GetTIPSummary(year);

            return View(viewModel);

        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public JsonResult GetAvailableRestoreProjects(int timePeriodId)
        {

            var result = new List<SelectListItem>();

            try
            {
                var availableProjects = _tipRepository.GetRestoreProjectList(timePeriodId);
                result.Add(new SelectListItem { Text = "", Value = "" });
                availableProjects.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.TipId + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Add(new SelectListItem { Text = "No Projects found", Value = "0" });
                return Json(new { response = result });

            }
            return Json(result);
        }

        #endregion

        #region TIP Status /TIP/Status/<tipyear>

        /// <summary>
        /// Display the status for a TIP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public ActionResult Status(string year)
        {
            // get the view model from the repo
            var viewModel = _tipRepository.GetTipStatusViewModel(year);
            return View("Status", viewModel);
        }

        /// <summary>
        /// Get the Reports View
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Reports(string year)
        {
            LoadSession();
            //Create the ViewModel
            ReportsViewModel model = new ReportsViewModel();
            model = _tipRepository.GetReportsViewModel(StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram), year);

            var val = model.ReportDetails.HasCurrentPolicy();
            return View("reports", model);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateReportProjectVersionSort(int reportId, string projects)
        {

            string error = String.Empty;
            try
            {
                _tipRepository.UpdateReportProjectVersionOrder(reportId, projects);
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

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult SetReportProjectVersionOnHold(int reportId, int projectVersionId)
        {

            string error = String.Empty;
            try
            {
                _tipRepository.SetReportProjectVersionOnHold(reportId, projectVersionId);
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

        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult DownloadReportList(string reportId, string referrerYear)
        {
            GridView grid = new GridView();
            grid.DataSource = _tipRepository.GetAlopReportResults(reportId);
            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=AlopResults.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();

            return RedirectToAction("Reports", new { @year = referrerYear });
        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult RenderAlopReport(string reportShortGuid, string reportFolder, string reportFormat)
        {
            string url = "http://sqlprod/reportserver?/" + reportFolder + "&rs:Command=Render&rs:Format=" + reportFormat + "&rc:Parameters=false&ReportID=";

            var reportId = ShortGuid.Decode(reportShortGuid);
            return Redirect(url + reportId.ToString());
        }

        /// <summary>
        /// Update the Status for a TIP
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public ActionResult UpdateStatus(StatusViewModel viewModel)
        {

            TipStatusModel model = new TipStatusModel();
            UpdateModel(model);

            if (!ModelState.IsValid)
            {
                viewModel.TipStatus = model;
                //return Json(new {foo = "bar"});
                return View("Status", viewModel);
            }

            //Send update to repo
            try
            {
                _tipRepository.UpdateTipStatus(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("TipController", "UpdateStatus", "TipStatusViewModel", ex);
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Changes successfully saved." });
        }

        #endregion

        #region Project Search

        /// <summary>
        /// Get the ProjectSearch view.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProjectSearch(string year)
        {
            LoadSession();
            var viewModel = new ProjectSearchViewModel();

            //Get the current criteria out of session and stuff into the view model
            //ApplicationState session = this.GetSession();
            viewModel = _tipRepository.GetProjectSearchViewModel(StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram), year);
            viewModel.ProjectSearchModel = this.GetProjectSearchModel();

            viewModel.TipSummary.TipYear = year;
            viewModel.TipSummary.TipYearTimePeriodID = (short)_tipRepository.GetYearId(year, Enums.TimePeriodType.TimePeriod);

            viewModel.ProjectSearchModel.TipYear = year;
            viewModel.ProjectSearchModel.TipYearID = viewModel.TipSummary.TipYearTimePeriodID;

            return View(viewModel);
        }

        private TIPSearchModel GetProjectSearchModel()
        {
            //This method will check to see if a ProjectSearchModel is already in exsitance in the Session object.
            //If so, it will copy it to the current object. If not, then defaults will be returned.

            //ToDo: Add a variable for the application, so that each application can specifiy its own defaults?

            var result = new TIPSearchModel();
            LoadSession();
            //Get a reference to session object
            //ApplicationState appSession = this.GetSession();

            //if (CurrentSessionApplicationState.ProjectSearchModel != null)
            //{
            //    result = (TIPSearchModel)CurrentSessionApplicationState.ProjectSearchModel;
            //}
            //else
            //{

            _tipRepository.SetProjectSearchDefaults(result);

            //Just return some general defaults for now
            //result.AmendmentStatus = "";
            result.AmendmentStatusID = null;
            result.COGID = "";
            result.ProjectName = "";
            result.ProjectType = "";
            result.FundingType = "";
            //result.SponsorAgency = "";
            result.SponsorAgencyID = null;
            result.TipID = "";
            result.StipId = String.Empty;
            //result.TipYear = "2008-2013";
            //result.TipYearID = 8; 
            //result.ImprovementType = "";
            result.ImprovementTypeID = null;
            result.VersionStatusId = (int)Enums.TIPVersionStatus.Active;
            //}

            return result;
        }

        /// <summary>
        /// Store the criteria and redirect to the ProjectList
        /// which will then apply the new criteria.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectSearch(ProjectSearchViewModel model)
        {
            LoadSession();
            CurrentSessionApplicationState.ProjectSearchModel = model.ProjectSearchModel;

            //Redirect to the project list. Determine TIP Year (text) for redirect from appSession
            return new RedirectToRouteResult(
                              new RouteValueDictionary {                              
                              { "controller", "TIP" },
                              { "action", "ProjectList" },
                              { "year", CurrentSessionApplicationState.CurrentTIP }                             
                        });
        }

        #endregion

        #region TIP Project List

        /// <summary>
        /// Returns a list of projects associated with this TIP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult ProjectList(string year, string dft, string df, int? page, int? amendmentTypeId, int? amendmentStatusId)
        {
            //DTS did NOT undersand this. The 'year' variable here is NOT a search criterium. -DBD
            //The 'df' variable is a dashboard (quick search) filter.

            LoadSession();
            //CurrentSessionApplicationState.ProjectSearchModel = null; 
            //Get a reference to session object
            //ApplicationState appSession = this.GetSession();
            //ApplicationState appSession = (ApplicationState)Session[DRCOGApp.SessionIdentifier];

            //Make a ProjectViewModel object from the search criteria
            var projectSearchModel = (CurrentSessionApplicationState.ProjectSearchModel as TIPSearchModel) ?? new TIPSearchModel();
            //CurrentSessionApplicationState.ProjectSearchModel = null;


            //If there is a 'df' dashboard filter, then the Session search criteria are reset.
            if (df != null)
            {
                //Reset Session search criteria
                //CurrentSessionApplicationState.ProjectSearchModel = null;


                //Assign dashboard search filter criteria
                projectSearchModel.TipYear = year;

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
                }

                //Assume from dashboard that we only want active projects.
                projectSearchModel.VersionStatusId = (int)Enums.TIPVersionStatus.Active;
            }
            //else if (projectSearchModel.AmendmentStatusID.HasValue)
            //{
            //    projectSearchModel.TipYear = year;
            //    projectSearchModel.AmendmentTypeId = (int)amendmentTypeId;
            //    projectSearchModel.AmendmentStatusID = amendmentStatusId;
            //}
            else if (amendmentTypeId != null)
            {
                projectSearchModel.TipYear = year;
                projectSearchModel.AmendmentTypeId = (int)amendmentTypeId;
                projectSearchModel.AmendmentStatusID = amendmentStatusId;
            }
            else
            {
                //Check to see if there is a projectSearchModel in Session. If not, then we have nt selected a dashboard or project search tab option.
                if ((CurrentSessionApplicationState.ProjectSearchModel as TIPSearchModel) != null)
                {
                    //Pull ProjectSearchModel from session and use
                    projectSearchModel = (TIPSearchModel)CurrentSessionApplicationState.ProjectSearchModel;
                }
                else
                {
                    //Create search using TipYear and Active Version only (default).
                    projectSearchModel.TipYear = year;
                    projectSearchModel.VersionStatusId = (int)Enums.TIPVersionStatus.Active;
                }

                if (!projectSearchModel.AmendmentTypeId.Equals(default(int)))
                {
                    projectSearchModel.VersionStatusId = (int)Enums.TIPVersionStatus.Pending;
                }

                projectSearchModel.AmendmentTypeId = amendmentTypeId.HasValue ? (int)amendmentTypeId : default(int);
                projectSearchModel.AmendmentStatusID = amendmentStatusId.HasValue ? (int)amendmentStatusId : default(int);
            }

            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.ProjectList = GetProjectList(projectSearchModel);
            viewModel.CurrentSponsors = _tipRepository.GetCurrentTimePeriodSponsorAgencies(year, Enums.ApplicationState.TIP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);

            var allowedTypes = new List<Enums.AmendmentType>();
            allowedTypes.Add(Enums.AmendmentType.Administrative);
            allowedTypes.Add(Enums.AmendmentType.Policy);
            viewModel.AmendmentTypes = new Dictionary<int, string>();
            foreach (Enums.AmendmentType type in allowedTypes)
            {
                viewModel.AmendmentTypes.Add((int)type, StringEnum.GetStringValue(type));
            }

            // ??? why save it in GetProjectList?
            CurrentSessionApplicationState.ProjectSearchModel = null;

            return View(viewModel);
        }



        /// <summary>
        /// Returns a list of projects associated with this TIP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult ProjectListCustom(TIPSearchModel projectSearchModel)
        {
            //DTS did NOT undersand this. The 'year' variable here is NOT a search criterium. -DBD
            //The 'df' variable is a dashboard (quick search) filter.

            LoadSession();

            projectSearchModel.VersionStatusId = (int)Enums.TIPVersionStatus.Active;

            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = projectSearchModel.TipYear;
            viewModel.ProjectList = GetProjectList(projectSearchModel);
            viewModel.CurrentSponsors = _tipRepository.GetCurrentTimePeriodSponsorAgencies(projectSearchModel.TipYear, Enums.ApplicationState.TIP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);

            return View(viewModel);
        }

        private IList<TipSummary> GetProjectList(TIPSearchModel projectSearchModel)
        {
            //Before passing the ProjectSearchModel, make sure it is validated
            projectSearchModel = this.ValidateSearchData((TIPSearchModel)projectSearchModel, StringEnum.GetStringValue(CurrentSessionApplicationState.CurrentProgram));

            var projectList = _tipRepository.GetTIPProjects(projectSearchModel);

            //Now save this projectSearchModel (for future searchs)
            CurrentSessionApplicationState.ProjectSearchModel = projectSearchModel;

            return projectList;
        }

        #endregion

        #region Delays

        public ActionResult Delays(string year, string id)
        {
            LoadSession();

            var viewModel = new DelaysViewModel();
            viewModel.TipSummary.TipYear = year;
            var tpId = _tipRepository.GetYearId(year, Enums.TimePeriodType.TimePeriod);
            viewModel.Delays = _tipRepository.GetDelays(id, tpId);

            viewModel.DelayYears = _tipRepository.GetDelayYears(tpId);
            viewModel.DelayYear = id;

            return View(viewModel);
        }

        public ActionResult DownloadDelays(string year, string id)
        {
            var timePeriodId = _tipRepository.GetYearId(year, Enums.TimePeriodType.TimePeriod);
            var collection = _tipRepository.GetDelays(id, timePeriodId);

            using (ExcelPackage pck = new ExcelPackage())
            {
                foreach (var affectLocation in collection.Select(x => x.AffectedProjectDelaysLocation).Distinct())
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(affectLocation);
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    var locationCollection = collection.Where(y => y.AffectedProjectDelaysLocation.Equals(affectLocation))
                        .Select(y => new { y.TipId, y.Sponsor, y.ProjectName, y.FederalAmount, y.Phase, y.IsInitiated, y.MidYearStatus, y.EndYearStatus, y.Notes, y.IsDelay });

                    ws.Cells["B2"].Value = "Candidate to have a Delayed Project Phase(s)";

                    using (ExcelRange r = ws.Cells["B2:D2"])
                    {
                        r.Merge = true;

                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                    }

                    ws.Cells["E2"].Value = "FY" + id + " Total";

                    using (ExcelRange r = ws.Cells["E2:J2"])
                    {
                        r.Merge = true;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                    }

                    // Load in delays
                    var cDelays = locationCollection.Where(y => !y.IsDelay);
                    if (cDelays.Count() > 0)
                    {
                        // There will be an error if you try and load in a record set that is 0
                        ws.Cells["B3"].LoadFromCollection(cDelays);
                    }

                    var nextRow = ws.Dimension.End.Row + 1;
                    ws.Cells[nextRow, 2].Value = "FY" + (Int32.Parse(id) - 1).ToString() + " First Year Delay Projects (subject to automatic deletion)";
                    using (ExcelRange r = ws.Cells[nextRow, 2, nextRow, 10])
                    {
                        r.Merge = true;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                    }

                    // Load in delays
                    var delays = locationCollection.Where(y => y.IsDelay);
                    if (delays.Count() > 0)
                    {
                        // There will be an error if you try and load in a record set that is 0
                        ws.Cells[nextRow + 1, 2].LoadFromCollection(delays);
                    }

                    ws.Cells["B1"].Value = "TIP ID";
                    ws.Cells["C1"].Value = "TIP Sponsor";
                    ws.Cells["D1"].Value = "TIP Project Name";
                    ws.Cells["E1"].Value = "Federal Funding ($1,000's)";
                    ws.Cells["F1"].Value = "Project Phase";
                    ws.Cells["G1"].Value = "Phase Initiated?(Y/N)";
                    ws.Cells["H1"].Value = "Mid-Year Status";
                    ws.Cells["I1"].Value = "End of Year Status";
                    ws.Cells["J1"].Value = "Additional Notes";

                    ws.Cells[1, 2, 1, 10].AutoFitColumns();
                    ws.Column(11).Hidden = true;
                    ws.Column(3).Width = 25;
                    ws.Column(4).AutoFit();
                    ws.Column(5).AutoFit();

                    var endRow = ws.Dimension.End.Row;
                    var endColumn = ws.Dimension.End.Column;

                    using (ExcelRange r = ws.Cells[1, 2, endRow, endColumn])
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    ws.View.ShowGridLines = false;
                    ws.InsertRow(1, 1);
                    ws.InsertRow(2, 1);
                    ws.Column(1).Width = 4;
                    using (ExcelRange r = ws.Cells["B1:J1"])
                    {
                        r.Merge = true;
                        r.Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                    }
                    ws.Cells["B1"].Value = "Potential DRCOG Projects Delayed for " + affectLocation + " - FY" + id;
                }

                // set some core property values
                pck.Workbook.Properties.Title = "Potential DRCOG Projects Delayed";
                pck.Workbook.Properties.Author = "DRCOG's Transportation Regional Improvement Projects and Survey (TRIPS)";
                pck.Workbook.Properties.Subject = "Potential DRCOG Projects Delayed";

                // set some extended property values
                pck.Workbook.Properties.Company = "Denver Regional Council of Governments";

                //Write it back to the client
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;  filename=DRCOG_Projects_Delayed.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }


            return RedirectToAction("Delays", new { year = year, id = id });
        }

        public ActionResult DelayUpdate(Delay model)
        {
            model = _tipRepository.GetDelay(model);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Partials/_DelaysUpdate", model);
            }
            return PartialView("Partials/_DelaysUpdate", model);
        }

        [HttpPost]
        public ActionResult DelayUpdate(FormCollection collection)
        {
            Delay model = new Delay();

            if (TryUpdateModel(model, new string[] { 
                "ProjectFinancialRecordId"
                , "FundingIncrementId"
                , "FundingResourceId"
                , "PhaseId"
                , "IsInitiated"
                , "IsChecked"
                , "MidYearStatus" 
                , "EndYearStatus" 
                , "ActionPlan" 
                , "MeetingDate" 
                , "Notes" 
                , "TimePeriodId" 
                , "TimePeriod"
                , "Year"
            }))
            {
                var ret = _tipRepository.UpdateDelay(model);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        message = "Form was not valid"
                        ,
                        error = "true"
                    });
                }
            }

            if (Request.IsAjaxRequest())
            {

                return Json(new
                {
                    message = "Delay updated successfully"
                    ,
                    error = "false"
                    ,
                    data = ""
                });
            }

            // A standard (non-Ajax) HTTP Post came in
            TempData["Message"] = "Delay updated successfully";
            return RedirectToAction("Delays", "TIP", new { @id = model.TimePeriod, @year = model.Year });
        }

        #endregion

        #region TIP Eligible Agencies Tab

        /// <summary>
        /// Display the eligible agencies lists for a TIP
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult Agencies(string year)
        {
            //get the mode from the repo
            SponsorsViewModel model = _tipRepository.GetSponsorsViewModel(year);

            return View(model);
        }

        /// <summary>
        /// Callback to see if an agency can be dropped from the tip
        /// </summary>
        /// <param name="year"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
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
        /// Update the eligible agencies associated with this TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateAgencies(string tipYear, List<int> added, List<int> removed)
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
                _tipRepository.UpdateEligibleAgencies(tipYear, added, removed);
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
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddEligibleAgency(string tipYear, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _tipRepository.AddAgencyToTimePeriod(tipYear, agencyId, Enums.ApplicationState.TIP);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an agency from the eligible agencies list
        /// if the agency does not sponsor any projects in the
        /// tip
        /// </summary>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult DropEligibleAgency(string tipYear, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _tipRepository.DropAgencyFromTimePeriod(tipYear, agencyId, Enums.ApplicationState.TIP);
            //TODO: Translate this to make sure it is not a SQL error. Do this or handle it client side?
            if (!jsr.Error.Equals(""))
            {
                jsr.Error = "That agency sponsors projects in the current TIP. They can not be removed.";
            }
            return Json(jsr);
        }

        #endregion

        #region TIP Amendments Tab

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public ActionResult Amendments(string year)
        {
            var viewModel = _tipRepository.GetAmendmentsViewModel(year); //new TipBaseViewModel();
            viewModel.RestoreYears = GetRestoreYears(year);
            //viewModel.TipSummary.TipYear = year;

            return View(viewModel);
        }

        #endregion

        #region TIP Pool List

        public ActionResult PoolList(string year)
        {
            //TODO: Implement - this is just scaffolding
            var viewModel = new PoolListViewModel();
            viewModel.TipSummary.TipYear = year;

            return View(viewModel);
        }

        #endregion

        #region FundingList Tab

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public ActionResult FundingList(string year, int? page)
        {

            var viewModel = new FundingSourceListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.TipSummary.TipYearTimePeriodID = (short)_tipRepository.GetYearId(year, Enums.TimePeriodType.TimePeriod);
            viewModel.FundingSources = _tipRepository.GetTipFundingSources(year); //.AsPagination(page.GetValueOrDefault(1), 10);   
            viewModel.FundingGroups = _tipRepository.AvailableFundingGroups();
            var sourceAgencies = _tipRepository.GetOrganizationsByType(Enums.OrganizationType.FundingSourceAgency);
            viewModel.SourceAgencies = new Dictionary<int, string>();
            foreach (Organization o in sourceAgencies)
            {
                viewModel.SourceAgencies.Add(new KeyValuePair<int, string>((int)o.OrganizationId, o.OrganizationName));
            }

            viewModel.RecipientAgencies = viewModel.SourceAgencies;
            return View("FundingList", viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult CreateFundingSource(FundingSourceModel model)
        {
            model.ProgramId = (int)Enums.ApplicationState.TIP;
            model.TimePeriodId = _tipRepository.GetYearId(model.TimePeriod, Enums.TimePeriodType.TimePeriod);

            bool result = false;
            try
            {
                _tipRepository.CreateFundingSource(model);
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
                message = "Funding successfully created."
                ,
                error = "false"
            });

        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult GetFundingSource(FundingSourceModel model)
        {
            //FundingSourceModel model = new FundingSourceModel() { FundingTypeId = guid, TimePeriodId = timePeriodId };
            FundingSourceModel result;
            try
            {
                result = _tipRepository.GetFundingSource(model);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = "Funding Source not found. An error has been logged."
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
                message = "Funding successfully retrieved."
                ,
                error = "false"
            });

        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateFundingSource(FundingSourceModel model)
        {
            model.ProgramId = (int)Enums.ApplicationState.TIP;
            model.TimePeriodId = _tipRepository.GetYearId(model.TimePeriod, Enums.TimePeriodType.TimePeriod);

            bool result = false;
            try
            {
                _tipRepository.UpdateFundingSource(model);
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
                message = "Funding successfully updated."
                ,
                error = "false"
            });

        }

        #endregion

        #region Amendment Mgt

        public ActionResult CreateAmendmentList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.ProjectList = _tipRepository.GetAmendableTIPProjects(year);

            return View(viewModel);
        }

        public ActionResult ManageAmendmentList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.ProjectList = _tipRepository.GetAmendableTIPProjects(year);

            return View(viewModel);
        }

        public ActionResult EditAmendmentList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.ProjectList = _tipRepository.GetActiveTIPProjects(year);

            return View(viewModel);
        }

        public ActionResult ProposedAmendmentList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            //viewModel.ProjectList = _tipRepository.GetProposedTIPProjects(year).Projects;

            return View(viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult Restore(string timePeriod, int id)
        {
            int result = default(int);
            try
            {
                IRestoreStrategy strategy = new RestoreStrategy(this._projectRepository, id).PickStrategy();
                result = (int)strategy.Restore(timePeriod);
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
                message = "Project successfully restored to " + timePeriod + "."
                ,
                error = "false"
            });
        }

        public ActionResult RestoreProjectList(string yearDestination, int yearSourceID, string tipID)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = yearDestination;

            //This is not going to work because we are going to need multiple search criteria.
            //var projectSearchModel = new TIPSearchModel();
            //projectSearchModel.TipYear = year;
            //viewModel.ProjectList = _tipRepository.GetTIPProjects(projectSearchModel);

            viewModel.ProjectList = _tipRepository.GetTIPRestoreCandidateProjects(yearDestination, yearSourceID, tipID);

            return View(viewModel);
        }

        public ActionResult WaitingProjectList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            viewModel.ProjectList = _tipRepository.GetWaitingListTIPProjects(year);

            return View(viewModel);
        }

        public ActionResult NonTipProjectList(string year)
        {
            var viewModel = new ProjectListViewModel();
            viewModel.TipSummary.TipYear = year;
            var projectSearchModel = new TIPSearchModel();
            projectSearchModel.TipYear = year;
            viewModel.ProjectList = _tipRepository.GetTIPProjects(projectSearchModel);

            return View(viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public JsonResult GetProjectsByAmendmentStatusId(string timePeriod, int amendmentStatusId)
        {

            var result = new List<TipSummary>();
            Enums.TIPAmendmentStatus amendmentStatus;
            try
            {
                amendmentStatus = (Enums.TIPAmendmentStatus)amendmentStatusId;
            }
            catch
            {
                result.Clear();
                return Json(new { response = "There was an error determining the Amendment Status" });
            }

            try
            {
                result = _tipRepository.GetProjectsByAmendmentStatusId(_tipRepository.GetYearId(timePeriod, Enums.TimePeriodType.TimePeriod), amendmentStatus).ToList();
            }
            catch (Exception ex)
            {
                result.Clear();
                return Json(new { response = result });

            }
            return Json(result);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult Amend(int projectVersionId)
        {
            int result = default(int);

            TipSummary summary = _projectRepository.GetProjectSummary(projectVersionId);

            ProjectAmendments amendment = new ProjectAmendments()
            {
                ProjectVersionId = projectVersionId
                ,
                PreviousProjectVersionId = summary.PreviousVersionId
            };
            try
            {
                amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
                IAmendmentStrategy strategy = new AmendmentStrategy(_projectRepository, amendment).PickStrategy();
                result = strategy.Amend();
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

        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult ResetSearchModel(string year)
        {
            LoadSession();
            CurrentSessionApplicationState.ProjectSearchModel = null;
            return RedirectToAction("ProjectList", new { tipYear = year });
        }

        #endregion

        #region PRIVATE HELPERS

        private TIPSearchModel ValidateSearchData(TIPSearchModel projectSearchModel, string currentProgram)
        {
            //Check completeness of TipYear
            if ((projectSearchModel.TipYearID == null) && (projectSearchModel.TipYear != null))
            {
                //Lookup the TipYearID
                projectSearchModel.TipYearID = _tipRepository.GetYearId(projectSearchModel.TipYear, Enums.TimePeriodType.TimePeriod);
            }

            if ((projectSearchModel.TipYearID != null) && (projectSearchModel.TipYear == null))
            {
                //Lookup the TipYear
                projectSearchModel.TipYear = _tipRepository.GetYear((int)projectSearchModel.TipYearID);
            }

            //Check completeness of SponsorAgency
            if ((projectSearchModel.SponsorAgencyID == null) && (projectSearchModel.SponsorAgency != null))
            {
                //Lookup the SponsorAgencyID
                projectSearchModel.SponsorAgencyID = _tipRepository.GetSponsorAgencyID(projectSearchModel.SponsorAgency);
            }

            if ((projectSearchModel.SponsorAgencyID != null) && (projectSearchModel.SponsorAgency == null))
            {
                //Lookup the SponsorAgency
                projectSearchModel.SponsorAgency = _tipRepository.GetSponsorAgency(projectSearchModel.SponsorAgencyID);
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
                projectSearchModel.AmendmentStatusID = _tipRepository.GetStatusID(projectSearchModel.AmendmentStatus, statusType);
            }

            if ((projectSearchModel.AmendmentStatusID != null) && (projectSearchModel.AmendmentStatus == null))
            {
                //Lookup the AmendmentStatus
                projectSearchModel.AmendmentStatus = _tipRepository.GetStatus(projectSearchModel.AmendmentStatusID, statusType);
            }

            //Check completeness of ImprovementType
            if ((projectSearchModel.ImprovementTypeID == null) && (projectSearchModel.ImprovementType != null))
            {
                //Lookup the ImprovementTypeID
                projectSearchModel.ImprovementTypeID = _tipRepository.GetImprovementTypeID(projectSearchModel.ImprovementType);
            }

            if ((projectSearchModel.ImprovementTypeID != null) && (projectSearchModel.ImprovementType == null))
            {
                //Lookup the ImprovementType
                projectSearchModel.ImprovementType = _tipRepository.GetImprovementType(projectSearchModel.ImprovementTypeID);
            }

            //Check completeness of ProjectType
            if ((projectSearchModel.ProjectTypeID == null) && (projectSearchModel.ProjectType != null))
            {
                //Lookup the ProjectTypeID
                projectSearchModel.ProjectTypeID = _tipRepository.GetProjectTypeID(projectSearchModel.ProjectType);
            }

            if ((projectSearchModel.ProjectTypeID != null) && (projectSearchModel.ProjectType == null))
            {
                //Lookup the ProjectType
                projectSearchModel.ProjectType = _tipRepository.GetProjectType(projectSearchModel.ProjectTypeID);
            }

            return projectSearchModel;
        }

        #endregion

    }
}
