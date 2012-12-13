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
            // copy this one since it is not on the form
            viewModel.InfoModel.SponsorId = viewModel.ProjectSponsorsModel.PrimarySponsor.OrganizationId;

            //Send update to repo
            try
            {
                RtpProjectRepository.UpdateProjectInfo(viewModel.InfoModel);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Project Info", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
