using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Formatting;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels.RTP.Project;

namespace Trips4.Controllers.Api
{
    public class RtpProjectInfoController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpProjectRepository RtpProjectRepository { get; set; }

        public RtpProjectInfoController(IRtpProjectRepository rprepo,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            RtpProjectRepository = rprepo;
        }

        [AuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public void Put(InfoViewModel viewModel)
        {
#if bozo
            int projectVersionId = viewModel.InfoModel.ProjectVersionId;
            string year = viewModel.InfoModel.RtpYear;

            // TODO: just use InfoModel
            //Get the model from the database
            InfoModel model = RtpProjectRepository.GetProjectInfo(projectVersionId, year);

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
            model.SponsorId = viewModel.ProjectSponsorsModel.PrimarySponsor.OrganizationId;
            model.SponsorNotes = viewModel.InfoModel.SponsorNotes;
            model.RtpYear = viewModel.InfoModel.RtpYear;
            model.TransportationTypeId = viewModel.InfoModel.TransportationTypeId;
            model.IsRegionallySignificant = viewModel.InfoModel.IsRegionallySignificant;
#endif
            // copy this one since it is not on the form
            viewModel.InfoModel.SponsorId = viewModel.ProjectSponsorsModel.PrimarySponsor.OrganizationId;

            //Send update to repo
            try
            {
                RtpProjectRepository.UpdateProjectInfo(viewModel.InfoModel);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Project Location", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
