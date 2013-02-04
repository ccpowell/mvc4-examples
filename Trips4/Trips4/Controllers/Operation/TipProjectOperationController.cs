using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Common.Services;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.ViewModels.TIPProject;
using DRCOG.TIP.Services;
using DRCOG.TIP.Services.RestoreStrategy.TIP;
using DTS.Web.MVC;
using Trips4.Configuration;

namespace Trips4.Controllers.Operation
{
    [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
    public class TipProjectOperationController : ApiController
    {

        private readonly ITipRepository TipRepository;
        private readonly IProjectRepository ProjectRepository;
        private readonly IFileRepositoryExtender FileRepository;
        private readonly DRCOGConfig Config;
        protected readonly ImageService ImageService;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public TipProjectOperationController(ITipRepository tipRepository,
            IProjectRepository projectRepository, IFileRepositoryExtender fileRepository, ITripsUserRepository userRepository)
        {
            TipRepository = tipRepository;
            ProjectRepository = projectRepository;
            FileRepository = fileRepository;
            Config = DRCOGConfig.GetConfig();
            ImageService = new ImageService(fileRepository);
        }

        public class Message { public string message { get; set; } }
        [HttpPost]
        public string Hello(Message message)
        {
            return message.message;
        }

        public class DeleteLocationMapRequest
        {
            public int ImageId { get; set; }
            public int ProjectVersionId { get; set; }
        }

        [HttpPost]
        public void DeleteLocationMap(DeleteLocationMapRequest request)
        {
            try
            {
                ImageService.Delete(request.ImageId, request.ProjectVersionId);
            }
            catch (Exception ex)
            {
                Logger.WarnException("TipProjectDeleteLocationMap failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        private void CheckError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = error });
            }
        }

        public class AgencyRequest
        {
            public int ProjectVersionId { get; set; }
            public int AgencyId { get; set; }
        }

        /// <summary>
        /// Add an agency to the current project as a Primary Sponsor
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        public void AddCurrent1Agency(AgencyRequest request)
        {
            var error = ProjectRepository.AddAgencyToTIPProject(request.ProjectVersionId, request.AgencyId, true);
            CheckError(error);
        }


        /// <summary>
        /// Add an agency to the current project as a Secondary Sponsor
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        public void AddCurrent2Agency(AgencyRequest request)
        {
            var error = ProjectRepository.AddAgencyToTIPProject(request.ProjectVersionId, request.AgencyId, false);
            CheckError(error);
        }

        /// <summary>
        /// Remove an Primary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        public void DropCurrent1Agency(AgencyRequest request)
        {
            var error = ProjectRepository.DropAgencyFromTIP(request.ProjectVersionId, request.AgencyId);
            CheckError(error);
        }

        /// <summary>
        /// Remove an Secondary Sponsor from the current project
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="agencyId"></param>
        /// <returns></returns>
        [HttpPost]
        public void DropCurrent2Agency(AgencyRequest request)
        {
            var error = ProjectRepository.DropAgencyFromTIP(request.ProjectVersionId, request.AgencyId);
            CheckError(error);
        }


        /// <summary>
        /// Add a municipality share to the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <param name="share"></param>
        /// <param name="isPrimary"></param>
        /// <returns></returns>
        [HttpPost]
        public void AddMuniShare(MunicipalityShareModel model)
        {
            try
            {
                model.Share = model.Share / 100;
                ProjectRepository.AddMunicipalityShare(model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        /// <summary>
        /// Drop a municpality share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        /// <returns></returns>
        [HttpPost]
        public void RemoveMuniShare(MunicipalityShareModel model)
        {
            try
            {
                ProjectRepository.DropMunicipalityShare(model.ProjectId.Value, model.MunicipalityId.Value);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        /// <summary>
        /// Add a county share record (ProjectCountyGeography table)
        /// </summary>
        [HttpPost]
        public void AddCountyShare(CountyShareModel model)
        {
            try
            {
                model.Share = model.Share / 100;
                ProjectRepository.AddCountyShare(model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        /// <summary>
        /// Remove a county share from the database
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        /// <returns></returns>
        [HttpPost]
        public void RemoveCountyShare(CountyShareModel model)
        {
            try
            {
                ProjectRepository.DropCountyShare(model.ProjectId.Value, model.CountyId.Value);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        [HttpPost]
        public int AddPoolProject(PoolProject model)
        {
            try
            {
                int poolProjectId = ProjectRepository.AddPoolProject(model);
                if (poolProjectId == 0)
                    throw new Exception("Returned 0 on poolMasterVersionId " + model.PoolMasterVersionID);
                return poolProjectId;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        [HttpPost]
        public void DeletePoolProject(PoolProject model)
        {
            try
            {
                ProjectRepository.DeletePoolProject(model.PoolProjectID);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        [HttpPost]
        public void UpdatePoolProject(PoolProject model)
        {
            try
            {
                ProjectRepository.UpdatePoolProject(model);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
