using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.TIP.Services.TIP;
using DRCOG.TIP.Services.DeleteStrategy.TIP;

namespace Trips4.Controllers.Api
{
    [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
    public class TipProjectAmendmentController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IProjectRepository ProjectRepository { get; set; }

        public TipProjectAmendmentController(IProjectRepository projectRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            ProjectRepository = projectRepository;
        }

        // Move an amendment along in its life cycle.
        // This used to be Amend in the ProjectController.
        // POST api/tipprojectamendment
        public int Post(ProjectAmendments amendment)
        {
            try
            {
                amendment.LocationMapPath = Trips4.Configuration.DRCOGConfig.GetConfig().LocationMapPath;
                IAmendmentStrategy strategy = new AmendmentStrategy(ProjectRepository, amendment).PickStrategy();
                int projectVersionId = strategy.Amend();
                return projectVersionId;
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Post TIP Project Amendment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        // Update an Amendment, with the ID of ProjectVersionId.
        // if AmendmentStatusId is 0, only Reason and Character are updated by the sproc.
        // PUT api/tipprojectamendment
        public void Put(ProjectAmendments value)
        {
            try
            {
                if (value.AmendmentStatusId != 0)
                {
                    ProjectRepository.UpdateProjectAmendmentStatus(value);
                }
                else
                {
                    ProjectRepository.UpdateAmendmentDetails(value);
                }
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Put TIP Project Amendment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        // If the amendment is in progress, just delete it.
        // If the amendment was approved, create another amendment for the deletion.
        // DELETE api/tipprojectamendment/5
        public int Delete(int id)
        {
            try
            {
                ProjectAmendments amendment = new ProjectAmendments()
                {
                    LocationMapPath = Trips4.Configuration.DRCOGConfig.GetConfig().LocationMapPath,
                    ProjectVersionId = id
                };

                IDeleteStrategy strategy = new DeleteStrategy(this.ProjectRepository, amendment).PickStrategy();
                int returnId = strategy.Delete();
                return returnId;
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Delete TIP Project Amendment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
