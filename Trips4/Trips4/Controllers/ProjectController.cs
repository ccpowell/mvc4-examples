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
using DRCOG.Domain.Models.TIPProject;
//using DRCOG.Domain.Models.TIPProject.Amendment;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIPProject;
using DRCOG.Web;
using DRCOG.Web.Filters;
using DTS.Web.MVC;
using DRCOG.Web.Configuration;
using DRCOG.Domain;
using Elmah;
using DRCOG.TIP.Services.TIP;
using DRCOG.TIP.Services.RestoreStrategy.TIP;
using DRCOG.Web.CustomResults;
using DRCOG.Common.Services.Interfaces;
using DRCOG.Common.Services;
using System.Web;
using DRCOG.Web.Utilities.ApplicationState;
using DRCOG.TIP.Services.DeleteStrategy.TIP;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using System.Transactions;
using DRCOG.Common.Web.MvcSupport.Attributes;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace DRCOG.Web.Controllers
{
    //[Authorize]
    //[RoleAuth]
    //[RemoteRequireHttps]
    public class ProjectController : ControllerBase
    {
        private readonly ITipRepository _tipRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileRepositoryExtender _fileRepository;
        private readonly DRCOGConfig _config;
        protected readonly ImageService ImageService;

        public ProjectController(ITipRepository tipRepository,
            IProjectRepository projectRepository, IFileRepositoryExtender fileRepository, IUserRepositoryExtension userRepository)
            : base("ProjectController", userRepository)
        {
            _tipRepository = tipRepository;
            _projectRepository = projectRepository;
            _fileRepository = fileRepository;
            _config = DRCOGConfig.GetConfig();
            ImageService = new ImageService(fileRepository);
        }

        private void LoadSession()
        {
            base.LoadSession(DRCOG.Domain.Enums.ApplicationState.TIP);
        }

        public JsonResult GetImprovementTypeMatch(int id)
        {
            return base.GetImprovementTypeMatch(id, _tipRepository);
        }

        public JsonResult GetProjectTypeMatch(int id)
        {
            return base.GetProjectTypeMatch(id, _tipRepository);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowPhoto(int id, int maxWidth = 348, int maxHeight = 480)
        {
            //This is my method for getting the image information
            // including the image byte array from the image column in
            // a database.
            Image image = ImageService.Load(id, maxWidth, maxHeight);
            //As you can see the use is stupid simple.  Just get the image bytes and the
            //  saved content type.  See this is where the contentType comes in real handy.
            ImageResult result = new ImageResult(image.Data, image.MediaType);

            return result;
        }


        /// <summary>
        /// The reports tab for a project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult Reports(int id, string year)
        {
            var model = new ProjectBaseViewModel();
            model = _projectRepository.GetDetailViewModel(id, year);
            return View("reports",model);
        }

        /// <summary>
        /// The Details tab for a project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult Details(int id, string year, string message)
        {
            var model = new DetailViewModel();
            model = _projectRepository.GetDetailViewModel(id, year);
            ViewData["message"] = message;
            return View("details", model);
        }

        public JsonResult CurrentTipIdDetails(string id, string year)
        {

            string error = String.Empty;

            VersionDetailsJson output;
            try
            {
                int pvId = _projectRepository.GetActiveProjectVersion(id);
                DetailViewModel model = _projectRepository.GetDetailViewModel(pvId, year);
                model.InfoModel.Image = null;


                output = new VersionDetailsJson() { 
                    InfoModel = model.InfoModel
                    , 
                    TipProjectFunding = model.TipProjectFunding 
                    ,
                    GeneralInfo = model.GeneralInfo
                    ,
                    ProjectUrl = Url.Action("Details", "Project", new { id = pvId, year = year })
                };

                //var serializer = new DataContractJsonSerializer(model.GetType());
                //output = String.Empty;
                //using (var ms = new MemoryStream())
                //{
                //    serializer.WriteObject(ms, model);
                //    output = Encoding.Default.GetString(ms.ToArray());
                //}


            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = String.IsNullOrEmpty(error) ? "Project could not be found." : error
                    ,
                    error = "true"
                    ,
                    exceptionMessage = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                message = "Successfully retreived the requested project."
                ,
                error = "false"
                ,
                data = output
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// This takes you to the most current version of a project
        /// </summary>
        /// <param name="id">TIP-ID</param>
        /// <param name="year">TimePeriod</param>
        /// <returns>Redirects to the Details page of a project</returns>
        public ActionResult Active(string id, string year)
        {
            var versionId = _projectRepository.GetActiveProjectVersion(id, year);

            return RedirectToAction("Details", new { controller = "Project", id = versionId });
        }

#region General Info

        /// <summary>
        /// Display the General Information for a project
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns></returns>
        public ActionResult Info(string year, int id, string message)
        {
            var viewModel = _projectRepository.GetProjectInfoViewModel(id, year);
            ViewData["message"] = message;
            return View(viewModel);            
        }

        /// <summary>
        /// Update the General Information for a project
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult UpdateInfo(InfoViewModel viewModel)
        {
            int projectVersionId = viewModel.InfoModel.ProjectVersionId.Value;
            string year = viewModel.InfoModel.TipYear;
            //Get the model from the database
            InfoModel model = _projectRepository.GetProjectInfo(projectVersionId, year);
            
            //Update it - UpdateModel was being wonky so it's a left/right-copy -DB    -- Did he say 'wonky'? Is that a word? -DBD
            model.AdministrativeLevelId = viewModel.InfoModel.AdministrativeLevelId;
            model.DRCOGNotes = viewModel.InfoModel.DRCOGNotes;
            model.ImprovementTypeId = viewModel.InfoModel.ImprovementTypeId;
            model.IsPoolMaster = viewModel.InfoModel.IsPoolMaster;
            model.ProjectId = viewModel.InfoModel.ProjectId;
            model.ProjectName = viewModel.InfoModel.ProjectName;
            model.ProjectPoolId = viewModel.InfoModel.ProjectPoolId;
            model.ProjectTypeId = viewModel.InfoModel.ProjectTypeId;
            model.ProjectVersionId = viewModel.InfoModel.ProjectVersionId;
            model.SelectionAgencyId = viewModel.InfoModel.SelectionAgencyId;
            model.SponsorContactId = viewModel.InfoModel.SponsorContactId;
            model.SponsorId = viewModel.InfoModel.SponsorId;
            model.SponsorNotes = viewModel.InfoModel.SponsorNotes;
            model.TipYear = viewModel.InfoModel.TipYear;
            model.TransportationTypeId = viewModel.InfoModel.TransportationTypeId;
            model.IsRegionallySignificant = viewModel.InfoModel.IsRegionallySignificant;
            model.STIPID = viewModel.InfoModel.STIPID;

            if (!ModelState.IsValid)
            {                
                return View("Info", viewModel);
            }

            //Send update to repo
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _projectRepository.UpdateProjectInfo(model);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdateInfo", "TipProjectInfoViewModel", ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new {message ="Changes successfully saved."});

        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult UpdateImage(FormCollection collection)
        {
            int projectVersionId = Int32.Parse(collection["ProjectVersionId"]);
            string year = collection["Year"];
            //Send update to repo
            Guid mediaId = default(Guid);
            string message = String.Empty;

            try
            {
                Image newImage = new Image();
                HttpPostedFileBase file = Request.Files["fileUpload"];

                if (file.ContentLength == 0)
                {
                    message = "No Image was selected.";
                }
                else if (file.ContentLength < 524288)
                {

                    newImage.Name = file.FileName;
                    newImage.MediaType = file.ContentType;

                    Int32 length = file.ContentLength;
                    byte[] tempImage = new byte[length];
                    file.InputStream.Read(tempImage, 0, length);
                    newImage.Data = tempImage;

                    mediaId = ImageService.Save(newImage, projectVersionId);
                }
                else
                {
                    message = "Image must be smaller then 4mb.";
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Location", new { controller = "Project", id = projectVersionId, tipyear = year, message = message.Equals(String.Empty) ? "Image was not updated." : message });
            }
            return RedirectToAction("Location", new { controller = "Project", id = projectVersionId, tipyear = year, message = message.Equals(String.Empty) ? "Image update successfully." : message });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DeleteLocationMap(int imageId, int projectVersionId)
        {
            try
            {
                ImageService.Delete(imageId, projectVersionId);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = "Error while deleting Location Map.",
                    error = "true",
                    exceptionMessage = ex.Message
                });
            }
            return Json(new
            {
                message = "Location Map successfully removed.",
                error = "false"
            });
        }

        /// <summary>
        /// Amend a project
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult Amend(AmendmentsViewModel amendmentViewModel)
        {
            /* Get Current Amendment Status
             * Does project need to be copied?
             * Amend Project
             * Check if Previous Active Amendment needs to be changed to inactive
             * Return to details page
             */

            ProjectAmendments amendment = amendmentViewModel.ProjectAmendments;
            //amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
            //IAmendmentStrategy strategy = new AmendmentStrategy(_projectRepository, amendment).PickStrategy();
            int projectVersionId = Amend(amendment);
            return RedirectToAction("Funding", new { controller = "Project", id = projectVersionId, message = "Amendment created successfully." });
        }

        private int Amend(ProjectAmendments amendment)
        {
            amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
            IAmendmentStrategy strategy = new AmendmentStrategy(_projectRepository, amendment).PickStrategy();
            int projectVersionId = strategy.Amend();
            return projectVersionId;
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult Amend(int projectVersionId, int previousVersionId)
        {
            ProjectAmendments amendment = new ProjectAmendments()
            {
                ProjectVersionId = projectVersionId
                ,
                PreviousProjectVersionId = previousVersionId
            };
            int result = Amend(amendment);
            //amendment.LocationMapPath = DRCOGConfig.GetConfig().LocationMapPath;
            //IAmendmentStrategy strategy = new AmendmentStrategy(_projectRepository, amendment).PickStrategy();
            //int projectVersionId = strategy.Amend();
            return RedirectToAction("Funding", new { controller = "Project", id = result, message = "Amendment processed successfully." });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult Restore(string year, int id)
        {
            //Restore projectVersionID (ID) to the given TIPYear (year)
            IRestoreStrategy strategy = new RestoreStrategy(this._projectRepository, id).PickStrategy();
            int returnId = (int)strategy.Restore(year);
            id = returnId != 0 ? returnId : id;

            return RedirectToAction("Funding", new { controller = "Project", id = id, message = "Project restored successfully." });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult DeleteAmendment(Int32 projectVersionId, Int32 previousProjectVersionId, string year)
        {
            ProjectAmendments amendment = new ProjectAmendments()
            {
                LocationMapPath = _config.LocationMapPath,
                ProjectVersionId = projectVersionId
            };

            IDeleteStrategy strategy = new DeleteStrategy(this._projectRepository,amendment).PickStrategy();
            int returnId = strategy.Delete();
            previousProjectVersionId = returnId != 0 ? returnId : previousProjectVersionId;

            if (!previousProjectVersionId.Equals(default(int)))
            {
                return RedirectToAction("Details", new { controller = "Project", id = previousProjectVersionId, message = "Amendment deleted successfully." });
            }
            else
                return RedirectToAction("ProjectList", new { controller = "Tip", tipYear = year });
        }

        /// <summary>
        /// Update the eligible agencies associated with this TIP Project. DEPRECATED
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="added"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
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
                _projectRepository.UpdateCurrentSponsors(projectVersionID, added, removed);
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
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddCurrent1Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _projectRepository.AddAgencyToTIPProject(projectVersionID, agencyId, true);
            return Json(jsr);
        }

        /// <summary>
        /// Add an agency to the current project as a Secondary Sponsor
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddCurrent2Agency(string tipYear, int projectVersionID, int agencyId)
        {

            var jsr = new JsonServerResponse();
            jsr.Error = _projectRepository.AddAgencyToTIPProject(projectVersionID, agencyId, false);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an Primary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DropCurrent1Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _projectRepository.DropAgencyFromTIP(projectVersionID, agencyId);
            return Json(jsr);
        }

        /// <summary>
        /// Remove an Secondary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DropCurrent2Agency(string tipYear, int projectVersionID, int agencyId)
        {
            var jsr = new JsonServerResponse();
            jsr.Error = _projectRepository.DropAgencyFromTIP(projectVersionID, agencyId);
            return Json(jsr);
        }

#endregion


        /// <summary>
        /// Display the Scope for a project
        /// </summary>
        /// <param name="id">project id</param>
        /// <returns></returns>
        public ActionResult Scope(string year, int id)
        {
            var viewModel = _projectRepository.GetScopeViewModel(id, year);
            //viewModel.ProjectSummary.IsEditable = false;            

            return View(viewModel);
        }

        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult UpdateScope(ScopeViewModel viewModel)
        {
            int projectVersionId = viewModel.TipProjectScope.ProjectVersionId;
            string year = viewModel.TipProjectScope.TipYear;
            //Get the model from the database
            ScopeModel model = _projectRepository.GetScopeModel(projectVersionId, year);
            //Update it
            model.BeginConstructionYear = viewModel.TipProjectScope.BeginConstructionYear;
            model.OpenToPublicYear = viewModel.TipProjectScope.OpenToPublicYear;
            model.ProjectDescription = viewModel.TipProjectScope.ProjectDescription;
            model.ProjectId = viewModel.TipProjectScope.ProjectId;
            model.ProjectVersionId = viewModel.TipProjectScope.ProjectVersionId;
            model.TipYear = viewModel.TipProjectScope.TipYear;

            if (!ModelState.IsValid)
            {
                return View("Scope", viewModel);
            }

            //Send update to repo
            try
            {
                _projectRepository.UpdateProjectScope(model);
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
        /// <param name="id">project id</param>
        /// <returns></returns>
        public ActionResult Location(string year, int id)
        {
            var viewModel = _projectRepository.GetProjectLocationViewModel(id, year);           
            return View(viewModel);
        }

        /// <summary>
        /// Update the Location information from the /Location view
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public ActionResult UpdateLocation()
        {
            //Manually parse up the form b/c of the muni & county split stuff
            int projectVersionId = Convert.ToInt32(Request.Form["ProjectVersionId"]);
            string year = Request.Form["TipYear"];
            //Get the existing model from the datagbase
            LocationModel model = _projectRepository.GetProjectLocationModel(projectVersionId, year);
            //Update values
            model.Limits = Request.Form["Limits"];
            model.FacilityName = Request.Form["FacilityName"];
            //model.CdotRegionId = 
            int cdotRegion = default(int), delaysLocation = default(int);
            Int32.TryParse(Request.Form["TipProjectLocation.CdotRegionId"], out cdotRegion);
            Int32.TryParse(Request.Form["TipProjectLocation.AffectedProjectDelaysLocationId"], out delaysLocation);
            
            model.CdotRegionId = cdotRegion;
            model.AffectedProjectDelaysLocationId = delaysLocation;

            //parse out the county & muni shares stuff... 
            Dictionary<int, CountyShareModel> countyShares = ExtractCountyShares(Request.Form);
            Dictionary<int, MunicipalityShareModel> muniShares = ExtractMuniShares(Request.Form);

            //Send updates to repo
            try
            {
                _projectRepository.UpdateProjectLocationModel(model);
                //Update the county shares
                foreach(CountyShareModel m in countyShares.Values)
                {
                    _projectRepository.UpdateCountyShare(m);
                }
                //Update the muni shares
                foreach (MunicipalityShareModel m in muniShares.Values)
                {
                    _projectRepository.UpdateMunicipalityShare(m);
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

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddPoolProject(int poolMasterVersionId, string projectName, string description, string beginAt, string endAt, decimal cost)
        {
            PoolProject model = new PoolProject()
            {
                PoolMasterVersionID = poolMasterVersionId
                , ProjectName = projectName
                , Description = description
                , BeginAt = beginAt
                , EndAt = endAt
                , Cost = cost
            };
            int poolProjectId = 0;

            try
            {
                poolProjectId = _projectRepository.AddPoolProject(model);
                if (poolProjectId == 0)
                    throw new Exception("Returned 0 on poolMasterVersionId" + poolMasterVersionId);

            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "AddPoolProject", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Pool Project successfully added.", poolprojectid = poolProjectId });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DeletePoolProject(int poolProjectId)
        {
            try
            {
                _projectRepository.DeletePoolProject(poolProjectId);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "DeletePoolProject", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Pool Project successfully removed." });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdatePoolProject(int poolProjectId, string projectName, string description, string beginAt, string endAt, decimal cost)
        {
            PoolProject model = new PoolProject()
            {
                PoolProjectID = poolProjectId
                , ProjectName = projectName
                , Description = description
                , BeginAt = beginAt
                , EndAt = endAt
                , Cost = cost
            };
            
            try
            {
                _projectRepository.UpdatePoolProject(model);
            }
            catch (Exception ex)
            {
                //this.Logger.LogMethodError("ProjectController", "UpdatePoolProject", Request.Form.ToString(), ex);
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Pool Project successfully updated." });
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
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Project Financial Record successfully added.", recordid = projectFinancialRecordId });
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
                return Json(new { error = "Changes could not be stored. An error has been logged." });
            }
            return Json(new { message = "Project Financial Record successfully removed." });
        }
        */

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateFinancialRecord(int financialRecordId, decimal previous, decimal future, decimal tipfunding, decimal federaltotal, decimal statetotal, decimal localtotal, decimal totalcost)
        {
            FundingModel model = new FundingModel()
            {
                ProjectFinancialRecordID = financialRecordId
                , Previous = (double)previous
                , Future = (double)future
                , Funding = (double)tipfunding
                , FederalTotal = (double)federaltotal
                , StateTotal = (double)statetotal
                , LocalTotal = (double)localtotal
                , TotalCost = (double)totalcost
            };

            try
            {
                _projectRepository.UpdateFinancialRecord(model);
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

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
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
                _projectRepository.UpdateFinancialRecordDetail(model);
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

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddFinancialRecordDetail(int projectVersionID, int fundingPeriodID, int fundingTypeID)
        {
            try
            {
                _projectRepository.AddFinancialRecordDetail(projectVersionID, fundingPeriodID, fundingTypeID);
            }
            catch (Exception ex)
            {
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

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DeleteFinancialRecordDetail(int projectVersionID, int fundingResourceId)
        {
            try
            {
                _projectRepository.DeleteFinancialRecordDetail(projectVersionID, fundingResourceId);
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
                message = "Funding Resouce removed."
                ,
                error = "false"
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
                return Json(new { message = "Changes could not be stored. An error has been logged."
                    , error = "true"
                    , exceptionMessage = ex.Message });
            }
            return Json(new { message = "Project Financial Record Detail successfully removed."
                , error = "false" });
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
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddCountyShare(int projectId, int countyId, double share, bool isPrimary)
        {
            CountyShareModel model = new CountyShareModel();
            model.ProjectId = projectId;
            model.CountyId = countyId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _projectRepository.AddCountyShare(model);
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
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult RemoveCountyShare(int projectId, int countyId)
        {
            try
            {
                _projectRepository.DropCountyShare(projectId,countyId);
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
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddMuniShare(int projectId, int muniId, double share, bool isPrimary)
        {
            MunicipalityShareModel model = new MunicipalityShareModel();
            model.ProjectId = projectId;
            model.MunicipalityId = muniId;
            model.Primary = isPrimary;
            model.Share = share / 100;
            try
            {
                _projectRepository.AddMunicipalityShare(model);
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
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult RemoveMuniShare(int projectId, int muniId)
        {
            try
            {
                _projectRepository.DropMunicipalityShare(projectId, muniId);
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
            var viewModel = _projectRepository.GetProjectLocationViewModel(id, tipYear);
            return Json(viewModel);
        }

        public ActionResult Amendments(string year, int id)
        {
            var viewModel = _projectRepository.GetAmendmentsViewModel(id, year);
            return View(viewModel);
        }

        public ActionResult Funding(string year, int id, string message)
        {
            var viewModel = _projectRepository.GetFundingViewModel(id, year);
            ViewData["message"] = message;
            return View(viewModel);
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult AddPhase(FundingPhase phase)
        {
            try
            {
                _projectRepository.AddPhase(phase);
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
                message = "Project Phase successfully added."
                ,
                error = "false"
            });
        }

        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult DeletePhase(FundingPhase phase)
        {
            try
            {
                _projectRepository.DeletePhase(phase);
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
                message = "Project Phase successfully removed."
                ,
                error = "false"
            });
        }


        public ActionResult Modeling(string year, int id)
        {
            var viewModel = _projectRepository.GetAmendmentsViewModel(id, year);
            return View(viewModel);
        }

        public ActionResult CDOTData(string year, int id)
        {
            var viewModel = _projectRepository.GetCDOTDataViewModel(id, year);
            return View(viewModel);
        }

        public ActionResult Strikes(string year, int id)
        {
            var viewModel = _projectRepository.GetStrikesViewModel(id, year);
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateAmendmentDetails(ProjectAmendments amendment)
        {
            try
            {
                _projectRepository.UpdateAmendmentDetails(amendment);
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
                message = "Amendment Detail updated successfully."
                ,
                error = "false"
            });
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RoleAuth(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateAvailableSponsorContacts(int id)
        {
            var result = new List<SelectListItem>();
            var contacts = _projectRepository.GetSponsorContacts(id);
            contacts.ToList().ForEach(x => { result.Add(new SelectListItem { Text = x.Value, Value = x.Key.ToString() }); });
            return new JsonResult
            {
                Data = result
            };

        }

        
    }
}
