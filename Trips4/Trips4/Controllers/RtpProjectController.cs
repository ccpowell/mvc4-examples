#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR	        REMARKS
 * 07/15/2009	DBouwman        1. Initial Creation (DTS). 
 * 02/03/2010   DDavidson       2. Multiple improvements.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using Trips4;
using DTS.Web.MVC;
using DRCOG.Domain;
//using Elmah;
using DRCOG.Domain.ViewModels.RTP.Project;
using DRCOG.Domain.ViewModels.RTP;
using DRCOG.Domain.Models.RTP;
using System.Data.SqlClient;
using System.Data;
using DRCOG.Domain.Helpers;
using Trips4.Utilities.ApplicationState;
using DRCOG.TIP.Services.DeleteStrategy.RTP;
//using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
//using DRCOG.Common.Web.MvcSupport.Attributes;
using Trips4.Configuration;

namespace Trips4.Controllers
{
    [Trips4.Filters.SessionAuthorizeAttribute]
    [Trips4.Filters.RouteData]
    public class RtpProjectController : ControllerBase
    {
        private readonly IRtpRepository _rtpRepository;
        private readonly IRtpProjectRepository _rtpProjectRepository;
        private readonly DRCOGConfig _config;

        public RtpProjectController(IRtpRepository rtpRepository, IRtpProjectRepository rtpProjectRepository, ITripsUserRepository userRepository)
            : base("RtpProjectController", userRepository)
        {
            _rtpRepository = rtpRepository;
            _rtpProjectRepository = rtpProjectRepository;
            _config = DRCOGConfig.GetConfig();
        }

        private void LoadSession()
        {
            base.LoadSession(Enums.ApplicationState.RTP);
        }

#if needed
        /// <summary>
        /// The reports tab for a project
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Reports(int id, string year)
        {
            if (!System.Web.Security.Roles.IsUserInRole("Administrator") &&
                !System.Web.Security.Roles.IsUserInRole("RTP Administrator"))
            {
                Redirect("http://www.drcog.org/index.cfm?page=RegionalTransportationPlan(RTP)");
            }

            var model = new ProjectBaseViewModel();
            model = _rtpProjectRepository.GetDetailViewModel(id, year);
            return View("reports",model);
        }
#endif
        /// <summary>
        /// The Details tab for a project
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult Details(int id, string year)
        {
            var model = new DetailViewModel();
            model = _rtpProjectRepository.GetDetailViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);
            
            return View(model);
        }

#region General Info

        /// <summary>
        /// Display the General Information for a project
        /// </summary>
        /// <param name="guid">project guid</param>
        /// <returns></returns>
        public ActionResult Info(string year, int id)
        {
            var model = _rtpProjectRepository.GetProjectInfoViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);            
        }


        public JsonResult GetImprovementTypeMatch(int id)
        {
            return base.GetImprovementTypeMatch(id, _rtpProjectRepository);
        }

        public JsonResult GetProjectTypeMatch(int id)
        {
            return base.GetProjectTypeMatch(id, _rtpProjectRepository);
        }

        ///// <summary>
        ///// Gets ProjectTypeId from ImprovementTypeId
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public JsonResult GetImprovementTypeMatch(int guid)
        //{
        //    IList<SqlParameter> sqlParms = new List<SqlParameter>();
        //    sqlParms.Add(new SqlParameter() { SqlDbType = SqlDbType.Int, ParameterName = "@Id", Value = guid });
        //    try
        //    {
        //        //var result = RtpProjectRepository.GetLookupSingle<String>("dbo.Lookup_GetProjectTypeByImprovementTypeId", "Value", sqlParms);
        //        var result = RtpProjectRepository.GetLookupCollection("dbo.Lookup_GetProjectTypeByImprovementTypeId", "Id", "Value", sqlParms);

        //        return Json(new { guid = result.First().Key, value = result.First().Value });
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Error = ex.Message;
        //        return Json(jsr);
        //    }
        //}

        ///// <summary>
        ///// Gets ImprovementTypeId from ProjectTypeId
        ///// </summary>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public JsonResult GetProjectTypeMatch(int guid)
        //{
        //    IList<SqlParameter> sqlParms = new List<SqlParameter>();
        //    sqlParms.Add(new SqlParameter("@Id", guid));
        //    try
        //    {
        //        var result = RtpProjectRepository.GetLookupSingle<Int32>("[dbo].[Lookup_GetImprovementTypeIdByProjectTypeId]", "ID", sqlParms);
        //        return new JsonResult
        //        {
        //            Data = result
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonServerResponse jsr = new JsonServerResponse();
        //        jsr.Error = ex.Message;
        //        return Json(jsr);
        //    }
        //}

        /// <summary>
        /// Create a completely new Project in the current TIP Year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        //[Trips4.Filters.SessionAuthorizeAttribute]
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Create(string year)
        //{
        //    var viewModel = _projectRepository.GetCreateProjectViewModel();
        //    viewModel.ProjectSummary = new Summary();
        //    viewModel.InfoModel = new InfoModel();
        //    viewModel.InfoModel.TipYear = year;
        //    viewModel.ProjectSummary.TipYear = year;
        //    return View(viewModel);
        //}

        //[Trips4.Filters.SessionAuthorizeAttribute]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create( ProjectVersionGeneralInfoModel model, string year)
        //{
        //    {
        //        /* Get Current Amendment Status
        //         * Does project need to be copied?
        //         * Amend Project
        //         * Check if Previous Active Amendment needs to be changed to inactive
        //         * Return to details page
        //         */
        //        model.AmendmentStatusID = (Int32)AmendmentStatus.Proposed;
        //        //model.ProjectVersionId = _projectRepository.CreateProject(model, year);
        //        //amendment = amendment.AmendProject();

        //        return RedirectToAction("Info", new { controller = "Project", guid = model.ProjectVersionId });
        //    }
        //}


        ///// <summary>
        ///// Store the newly created Project and redirect the user
        ///// to /Project/{tipyear}/Info/{pvid]
        ///// </summary>
        ///// <returns></returns>
        //[Trips4.Filters.SessionAuthorizeAttribute]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create_orginal(InfoViewModel model)
        //{
        //    #region Validation
        //    if (model.InfoModel.ProjectName == "")
        //    {
        //        ModelState.AddModelError("ProjectName", "Project must have a name!");
        //    }
        //    if (model.InfoModel.SponsorId.HasValue == false)
        //    {
        //        ModelState.AddModelError("SponsorId", "You must select a sponsor!");
        //    }
        //    if (model.InfoModel.SponsorContactId.HasValue == false)
        //    {
        //        ModelState.AddModelError("SponsorContactId", "You must select a sponsor contact!");
        //    }
        //    if (model.InfoModel.AdministrativeLevelId.HasValue == false)
        //    {
        //        ModelState.AddModelError("AdministrativeLevelId", "You must select an Admin Level!");
        //    }
        //    if (model.InfoModel.ImprovementTypeId.HasValue == false)
        //    {
        //        ModelState.AddModelError("ImprovementTypeId", "You must select an Improvement Type!");
        //    }
        //    if (model.InfoModel.TransportationTypeId.HasValue == false)
        //    {
        //        ModelState.AddModelError("TransportationTypeId", "You must select a Transportation Type!");
        //    }

        //    if (model.InfoModel.SponsorNotes == "")
        //    {
        //        ModelState.AddModelError("SponsorNotes", "You must provide some sponsor notes!");
        //    }
        //    #endregion 

        //    //If the model is not valid...
        //    if (!ModelState.IsValid)
        //    {
        //        var viewModel = _projectRepository.GetCreateProjectViewModel();
        //        //pass across the model filled with form values...
        //        viewModel.ProjectSummary = model.ProjectSummary;
        //        //If we have a sponsorId, then we need to also fill
        //        //the AvailableSponsorContacts list
        //        if (viewModel.InfoModel.SponsorId.HasValue)
        //        {
        //            viewModel.AvailableSponsorContacts = _projectRepository.GetSponsorContacts(viewModel.InfoModel.SponsorId.Value);
        //        }
                
        //        return View(viewModel);
        //    }
        //    else
        //    {
        //        //store and redirect to the new project version
        //        int pvId = _projectRepository.CreateProject(model.InfoModel);
        //        string year = model.InfoModel.TipYear;
        //        return RedirectToAction("Info", new { controller = "Project", tipyear = year, guid = pvId });
        //    }

        //}

        /// <summary>
        /// Amend a project
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public ActionResult Amend(AmendmentsViewModel amendmentViewModel /*Int32 projectVersionId, String year2*/)
        {
            /* Get Current Amendment Status
             * Does project need to be copied?
             * Amend Project
             * Check if Previous Active Amendment needs to be changed to inactive
             * Return to details page
             */
            //ProjectAmendments amendment = new ProjectAmendments();
            //amendment = amendmentViewModel.ProjectAmendments;
            //if (amendment.RequiresProjectCopy(_projectRepository.GetProjectAmendmentStatus((Int32)amendment.ProjectVersionId)))
            //{
            //    amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
            //    amendment.PreviousProjectVersionId = (Int32)amendment.ProjectVersionId; 
            //    amendment.ProjectVersionId = _projectRepository.CopyProject(amendment.PreviousProjectVersionId);
            //}
            //amendment = amendment.AmendProject();
            
            //_projectRepository.UpdateProjectAmendmentStatus(amendment);
            //if (amendment.RequireVersionStatusUpdate())
            //{
            //    _projectRepository.UpdateProjectVersionStatusId((Int32)amendment.PreviousProjectVersionId, amendment.VersionStatusId);
            //}

            throw new NotImplementedException();

            //ProjectAmendments amendment = amendmentViewModel.ProjectAmendments;
            //amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
            //IAmendmentStrategy strategy = new DRCOG.TIP.Services.AmendmentStrategy(RtpProjectRepository, amendment).PickStrategy();
            //int projectVersionId = strategy.Amend();

            //return RedirectToAction("Details", new { controller = "Project", guid = projectVersionId });
            //return RedirectToAction("Details", new { controller = "Project", guid = amendment.ProjectVersionId });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public ActionResult DeleteAmendment(Int32 projectVersionId, Int32 previousProjectVersionId)
        {
            //throw new NotImplementedException();
            CycleAmendment amendment = new CycleAmendment()
            {
                //LocationMapPath = _config.LocationMapPath,
                ProjectVersionId = projectVersionId
            };
            
            IDeleteStrategy strategy = new DeleteStrategy(this._rtpProjectRepository, amendment).PickStrategy();
            int returnId = strategy.Delete();
            previousProjectVersionId = !returnId.Equals(default(int)) ? returnId : previousProjectVersionId;

            if (!previousProjectVersionId.Equals(default(int)))
            {
                return RedirectToAction("Details", new { controller = "RtpProject", id = previousProjectVersionId });
            }

            string returnUrl = HttpContext.Request.UrlReferrer.PathAndQuery ?? String.Empty;

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", new { controller = "Rtp", year = String.Empty });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DropAmendment(CycleAmendment amendment)
        {
            try
            {
                amendment.AmendmentStatusId = (int)Enums.RTPAmendmentStatus.Cancelled;

                IDeleteStrategy strategy = new DeleteStrategy(this._rtpProjectRepository, amendment).PickStrategy();
                strategy.Delete();
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
                message = "Project successfully updated."
                ,
                error = "false"
            });
        }

        /// <summary>
        /// Update the eligible agencies associated with this TIP Project. DEPRECATED
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public JsonResult UpdateAgencies(string projectVersionID, List<int> added, List<int> removed)
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
                _rtpProjectRepository.UpdateCurrentSponsors(projectVersionID, added, removed);
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
        /// Add an agency to the current project as a Primary Sponsor
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public JsonResult AddCurrent1Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _rtpProjectRepository.AddAgencyToTIPProject(projectVersionID, agencyId, true);
            return Json(jsr);
        }

        /// <summary>
        /// Add an agency to the current project as a Secondary Sponsor
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public JsonResult AddCurrent2Agency(string tipYear, int projectVersionID, int agencyId)
        {

            var jsr = new JsonServerResponse();
            jsr.Error = _rtpProjectRepository.AddAgencyToTIPProject(projectVersionID, agencyId, false);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an Primary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public JsonResult DropCurrent1Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _rtpProjectRepository.DropAgencyFromTIP(projectVersionID, agencyId);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an Secondary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public JsonResult DropCurrent2Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _rtpProjectRepository.DropAgencyFromTIP(projectVersionID, agencyId);
            return Json(jsr);
        }

#endregion


        /// <summary>
        /// Display the Scope for a project
        /// </summary>
        /// <param name="guid">project guid</param>
        /// <returns></returns>
        public ActionResult Scope(string year, int id)
        {
            var model = _rtpProjectRepository.GetScopeViewModel(id, year);
            //viewModel.ProjectSummary.IsEditable = false;            

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        [HttpPost]
        public ActionResult UpdateScope(ScopeViewModel viewModel)
        {
            int projectVersionId = viewModel.RtpProjectScope.ProjectVersionId;
            string year = viewModel.RtpProjectScope.RtpYear;
            //Get the model from the database
            ScopeModel model = _rtpProjectRepository.GetScopeModel(projectVersionId, year);
            //Update it
            //model.BeginConstructionYear = viewModel.RtpProjectScope.BeginConstructionYear;
            //model.OpenToPublicYear = viewModel.RtpProjectScope.OpenToPublicYear;
            model.ProjectDescription = viewModel.RtpProjectScope.ProjectDescription;
            model.ShortDescription = viewModel.RtpProjectScope.ShortDescription;
            model.ProjectId = viewModel.RtpProjectScope.ProjectId;
            model.ProjectVersionId = viewModel.RtpProjectScope.ProjectVersionId;
            model.RtpYear = viewModel.RtpProjectScope.RtpYear;

            if (!ModelState.IsValid)
            {
                return View("Scope", viewModel);
            }

            //Send update to repo
            try
            {
                _rtpProjectRepository.UpdateProjectScope(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdateScope", "ScopeViewModel", ex);
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Changes successfully saved." });
        }

        /// <summary>
        /// Display the Location for a project
        /// </summary>
        /// <param name="guid">project guid</param>
        /// <returns></returns>
        public ActionResult Location(string year, int id)
        {
            var model = _rtpProjectRepository.GetProjectLocationViewModel(id, year);
            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);
            return View(model);
        }


        public JsonResult GetSegmentDetails(int segmentId)
        {
            SegmentModel model;
            try
            {
                model = _rtpProjectRepository.GetSegmentDetails(segmentId);
                model.LRSSchemeBase = _rtpProjectRepository.GetLRSScheme((int)SchemeName.LRSProjects);
                XMLService xml = new XMLService(_rtpProjectRepository);
                model._LRS = xml.LoadRecords((int)SchemeName.LRSProjects, segmentId);
                //model._LRS = xml.LoadRecord((int)SchemeName.LRSProjects, segmentId);
                //var test = xml.GetScheme(1);
                //var temp = xml.GenerateXml(test,xml.GetSegmentLRSData(1, segmentId));
            }
            catch (Exception ex)
            {
                return Json(new { message = "Segment not found." });
            }
            return Json(model);
        }

        public JsonResult GetSegmentLRS(int segmentId)
        {
            SegmentModel model = new SegmentModel();
            try
            {
                model.LRSSchemeBase = _rtpProjectRepository.GetLRSScheme((int)SchemeName.LRSProjects);
                XMLService xml = new XMLService(_rtpProjectRepository);
                model._LRS = xml.LoadRecords((int)SchemeName.LRSProjects, segmentId);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Segment not found." });
            }
            return Json(model);
        }

        public JsonResult GetSegmentLRSDetails(int LRSId)
        {
            SegmentModel model = new SegmentModel();
            try
            {
                XMLService xml = new XMLService(_rtpProjectRepository);
                model.LRSRecord = xml.LoadRecord((int)SchemeName.LRSProjects, LRSId);
            }
            catch (Exception ex)
            {
                return Json(new { message = "LRS Record not found." });
            }
            return Json(model);
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddSegment(SegmentModel model)
        {
            int segmentId = 0;

            try
            {
                segmentId = _rtpProjectRepository.AddSegment(model);
                if (segmentId == 0)
                    throw new Exception("Returned 0 on projectVersionId" + model.ProjectVersionId);

            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully added.", segmentId = segmentId });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DeleteSegment(int segmentId)
        {
            try
            {
                _rtpProjectRepository.DeleteSegment(segmentId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully removed." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddLRSRecord(SegmentModel model)
        {
            try
            {
                XMLService xml = new XMLService(_rtpProjectRepository);
                if (!String.IsNullOrEmpty(model.LRSRecordRaw))
                {
                    string data = xml.GenerateXml(xml.GetScheme((int)SchemeName.LRSProjects), new LRSRecord() { Columns = model.LRSRecordRaw.ToDictionary(',') });
                    model.LRSxml = data;

                    _rtpProjectRepository.AddLRSRecord(model);
                }
                else return Json(new { message = "Changes were not found. Refresh your page and try again." });

            }
            catch (Exception ex)
            {
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DeleteLRSRecord(int lrsId)
        {
            try
            {
                _rtpProjectRepository.DeleteLRSRecord(lrsId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "LRS Record successfully removed." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateLRSRecord(SegmentModel model)
        {
            try
            {
                XMLService xml = new XMLService(_rtpProjectRepository);
                if (!String.IsNullOrEmpty(model.LRSRecordRaw))
                {
                    string data = xml.GenerateXml(xml.GetScheme((int)SchemeName.LRSProjects), new LRSRecord() { Columns = model.LRSRecordRaw.ToDictionary(',') });
                    model.LRSxml = data;

                    _rtpProjectRepository.UpdateLRSRecord(model);
                }
                else return Json(new { message = "Changes were not found. Refresh your page and try again." });
                
            }
            catch (Exception ex)
            {
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateSegment(SegmentModel model)
        {
            try
            {
                XMLService xml = new XMLService(_rtpProjectRepository);
                if (!String.IsNullOrEmpty(model.LRSRecordRaw))
                {
                    string data = xml.GenerateXml(xml.GetScheme((int)SchemeName.LRSProjects), new LRSRecord() { Columns = model.LRSRecordRaw.ToDictionary(',') });
                    model.LRSxml = data;
                } else model.LRSxml = xml.GenerateXml(null, null);
                _rtpProjectRepository.UpdateSegment(model);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateSegmentSummary(SegmentModel model)
        {
            try
            {
                _rtpProjectRepository.UpdateSegmentSummary(model);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Segment successfully updated." });
        }

        /*
        public JsonResult AddFinancialRecord(decimal previous, decimal future, decimal tipfunding, decimal federaltotal, decimal statetotal, decimal localtotal, decimal totalcost)
        {
            FundingModel model = new FundingModel()
            {
                Previous = (double)previous
                , Future = (double)future
                , TIPFunding = (double)tipfunding
                , FederalTotal = (double)federaltotal
                , StateTotal = (double)statetotal
                , LocalTotal = (double)localtotal
                , TotalCost = (double)totalcost
            };
            int projectFinancialRecordId = 0;

            try
            {
                projectFinancialRecordId = _projectRepository.AddFinancialRecord(model);
                if (projectFinancialRecordId == 0)
                    throw new Exception("Returned 0 on Record");
            }
            catch (Exception ex)
            {
                this.Logger.LogMethodError("ProjectController", "AddFinancialRecord", Request.Form.ToString(), ex);
                return Json(new { Error = "Changes could not be stored. An Error has been logged." });
            }
            return Json(new { Message = "Project Financial Record successfully added.", recordid = projectFinancialRecordId });
        }
        */

        /*
        public JsonResult DeleteFinancialRecord(int financialRecordId)
        {
            try
            {
                _projectRepository.DeleteFinancialRecord(financialRecordId);
            }
            catch (Exception ex)
            {
                this.Logger.LogMethodError("ProjectController", "DeleteFinancialRecord", Request.Form.ToString(), ex);
                return Json(new { Error = "Changes could not be stored. An Error has been logged." });
            }
            return Json(new { Message = "Project Financial Record successfully removed." });
        }
        */

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateFinancialRecord(Funding model)//(int projectVersionId, decimal constantCost, decimal VisionCost, decimal yoeCost)
        {
            //Funding model = new Funding()
            //{
            //    ProjectVersionId = projectVersionId
            //    ,
            //    ConstantCost = (Decimal)constantCost
            //    ,
            //    VisionCost = (Decimal)VisionCost
            //    ,
            //    YOECost = (Decimal)yoeCost
            //};

            try
            {
                _rtpProjectRepository.UpdateFinancialRecord(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdateFinancialRecord", Request.Form.ToString(), ex);
                return Json(new { message = "Changes could not be stored. An error has been logged."
                    , error = "true"
                    , exceptionMessage = ex.Message });
            }
            return Json(new { message = "Project Financial Record successfully updated."
                , error = "false" });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateFinancialRecordDetail(int projectFinancialRecordID, int fundingTypeID, int fundingLevelID, int fundingPeriodID, decimal incr01, decimal incr02, decimal incr03, decimal incr04, decimal incr05)
        {
            ProjectFinancialRecordDetail model = new ProjectFinancialRecordDetail()
            {
                ProjectFinancialRecordID = projectFinancialRecordID
               , FundingLevelID = fundingLevelID
               , FundingTypeID = fundingTypeID
               , FundingPeriodID = fundingPeriodID
               , Incr01 = incr01
               , Incr02 = incr02
               , Incr03 = incr03
               , Incr04 = incr04
               , Incr05 = incr05
            };

            try
            {
                _rtpProjectRepository.UpdateFinancialRecordDetail(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdateFinancialRecordDetail", Request.Form.ToString(), ex);
                return Json(new { message = "Changes could not be stored. An error has been logged."
                    , error = "true"
                    , exceptionMessage = ex.Message });
            }
            return Json(new { message = "Project Financial Record Detail successfully updated."
                , error = "false" });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddFinancialRecordDetail(int projectVersionID, int fundingPeriodID, int fundingTypeID)
        {
            try
            {
                _rtpProjectRepository.AddFinancialRecordDetail(projectVersionID, fundingPeriodID, fundingTypeID);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddFinancialRecordDetail", Request.Form.ToString(), ex);
                return Json(new
                {
                    message = "Changes could not be stored. An error has been logged."
                    , error = "true"
                    , exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Project Financial Record Detail successfully added."
                , error = "false"
            });
        }

        /*
        public JsonResult DeleteFinancialRecordDetail(int projectFinancialRecordId, int fundingTypeID, int fundingLevelID, int fundingPeriodID)
        {
            ProjectFinancialRecordDetail model = new ProjectFinancialRecordDetail()
            {
                ProjectFinancialRecordID = projectFinancialRecordId
               , FundingLevelID = fundingLevelID
               , FundingTypeID = fundingTypeID
               , FundingPeriodID = fundingPeriodID
            };

            try
            {
                _projectRepository.DeleteFinancialRecordDetail(model);
            }
            catch (Exception ex)
            {
                this.Logger.LogMethodError("ProjectController", "DeleteFinancialRecordDetail", Request.Form.ToString(), ex);
                return Json(new { Message = "Changes could not be stored. An Error has been logged."
                    , Error = "true"
                    , exceptionMessage = ex.Message });
            }
            return Json(new { Message = "Project Financial Record Detail successfully removed."
                , Error = "false" });
        }
        */

        //public PartialViewResult FinancialRecordDetails()
        //{
        //    ViewData.Model = new FundingDetailPivotModel()
        //    {
                
        //    };
        //    return PartialView("~/Views/Project/Partials/ProjectFundingDetailPartial.ascx", Model.FundingDetailPivotModel);
        //}

        /// <summary>
        /// Add a county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddCountyShare(int projectId, int countyId, double share, bool isPrimary)
        {
            CountyShareModel model = new CountyShareModel();
            model.ProjectId = projectId;
            model.CountyId = countyId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _rtpProjectRepository.AddCountyShare(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "County successfully added." });

        }

        /// <summary>
        /// Remove a county share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult RemoveCountyShare(int projectId, int countyId)
        {
            try
            {
                _rtpProjectRepository.DropCountyShare(projectId, countyId);
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
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddMuniShare(int projectId, int muniId, double share, bool isPrimary)
        {
            MunicipalityShareModel model = new MunicipalityShareModel();
            model.ProjectId = projectId;
            model.MunicipalityId = muniId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _rtpProjectRepository.AddMunicipalityShare(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddCountyShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Municipality successfully added." });
        }

        /// <summary>
        /// Drop a municpality share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult RemoveMuniShare(int projectId, int muniId)
        {
            try
            {
                _rtpProjectRepository.DropMunicipalityShare(projectId, muniId);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "RemoveMuniShare", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Record successfully dropped." });
        }

        /// <summary>
        /// Parse the County Shares from the Form Parameter collection 
        /// </summary>
        /// <param name="formParams"></param>
        /// <returns></returns>
        public Dictionary<int, CountyShareModel> ExtractCountyShares(NameValueCollection formParams)
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
        public Dictionary<int, MunicipalityShareModel> ExtractMuniShares(NameValueCollection formParams)
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
        
        public JsonResult MuniShares(string tipYear, int id)
        {
            var viewModel = _rtpProjectRepository.GetProjectLocationViewModel(id, tipYear);
            return Json(viewModel);
        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        public ActionResult Amendments(string year, int id)
        {
            var model = _rtpProjectRepository.GetAmendmentsViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.RtpSummary.RtpYear);
            pp.Add("ProjectVersionId", model.RtpSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);
        }

        public ActionResult Funding(string year, int id)
        {
            var model = _rtpProjectRepository.GetFundingViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);
        }

        public JsonResult GetPlanReportGroupingCategoryDetails(int id)
        {
            try
            {
                var result = _rtpProjectRepository.GetPlanReportGroupingCategoryDetails(id);
                return Json(result);
            }
            catch (Exception ex)
            {
                JsonServerResponse jsr = new JsonServerResponse();
                jsr.Error = ex.Message;
                return Json(jsr);
            }
        }
#if wtf
        public ActionResult Modeling(string year, int id)
        {
            var model = _rtpProjectRepository.GetAmendmentsViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);
        }
#endif
        public ActionResult CDOTData(string year, int id)
        {
            var model = _rtpProjectRepository.GetCDOTDataViewModel(id, year);

            // set page parameters for javascript
            var pp = CreatePageParameters();
            pp.Add("RtpYear", model.ProjectSummary.RtpYear);
            pp.Add("ProjectVersionId", model.ProjectSummary.ProjectVersionId);
            SetPageParameters(pp);

            return View(model);
        }

        [Trips4.Filters.SessionAuthorizeAttribute]
        //public ActionResult Strikes(string year, int guid)
        //{
        //    var viewModel = RtpProjectRepository.GetStrikesViewModel(guid, year);
        //    return View(viewModel);
        //}
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult UpdateAvailableSponsorContacts(int id)
        {
            var result = new List<SelectListItem>();
            //result.Add(new SelectListItem { Value = "1", Text = "Nick Kirkes" });
            var contacts = _rtpProjectRepository.GetSponsorContacts(id);
            contacts.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            return new JsonResult
            {
                Data = result
            };

        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult AddFundingSource(int fundingResourceId, int projectVersionId)
        {
            var fundingSource = new FundingSource() { Id = fundingResourceId };
            try
            {
                _rtpProjectRepository.AddFundingSource(fundingSource, projectVersionId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Funding Source successfully Added." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult DeleteFundingSource(int fundingResourceId, int projectVersionId)
        {
            var fundingSource = new FundingSource() { Id = fundingResourceId };
            try
            {
                _rtpProjectRepository.DeleteFundingSource(fundingSource, projectVersionId);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Funding Source successfully removed." });
        }

        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public JsonResult CreateCategory(string categoryName, string shortName, string description, string plan)
        {
            int result;
            try
            {
                result = _rtpRepository.CreateCategory(categoryName, shortName, description, plan);
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
                message = categoryName + " created successfully."
                ,
                error = "false"
            });
        }
        
    }
}
