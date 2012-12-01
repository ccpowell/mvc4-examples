using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;
using System.Net.Http.Formatting;
using DRCOG.Domain.Interfaces;

namespace Trips4.Controllers.Api
{
    public class SurveyInfoController : ApiController
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private ISurveyRepository SurveyRepository { get; set; }

        public SurveyInfoController(ISurveyRepository surveyRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            SurveyRepository = surveyRepository;
        }

        [AuthorizeAttribute(Roles = "Administrator, Survey Administrator, Sponsor")]
        public void Put(DRCOG.Domain.ViewModels.Survey.InfoViewModel viewModel)
        {
            // not in the form where we expect it
            viewModel.Project.SponsorId = viewModel.ProjectSponsorsModel.PrimarySponsor.OrganizationId.Value;
            viewModel.Project.TimePeriod = viewModel.Current.Name;

            //Send update to repo
            try
            {
                SurveyRepository.UpdateProjectInfo(viewModel.Project);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update Survey Info", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
