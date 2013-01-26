using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Interfaces;

namespace Trips4.Controllers.Operation
{
    public class MiscController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpRepository RtpRepository { get; set; }
        private ISurveyRepository SurveyRepository { get; set; }

        public MiscController(Trips4.Data.TripsRepository trepo, IRtpRepository rrepo, ISurveyRepository srepo)
        {
            TripsRepository = trepo;
            RtpRepository = rrepo;
            SurveyRepository = srepo;
        }


        public class GeneralResult
        {
            public bool Error { get; set; }
            public string Message { get; set; }
        }

        
        [HttpPost]
        public GeneralResult LoginResetPassword(string email)
        {
            if (true)
            {
                return new GeneralResult()
                {
                    Message = "Password reset is not implemented. Please call the TRIPS administrator.",
                    Error = true
                };
            }
            else
            {
                return new GeneralResult()
                {
                    Message = "Your password has been reset. An email will be sent with your new password.",
                    Error = false
                };
            }
        }


        public class RtpGetAmendableProjectsRequest
        {
            public int rtpPlanYearId;
            public int cycleId;
        }

        [HttpPost]
        public IEnumerable<System.Web.Mvc.SelectListItem> RtpGetAmendableProjects(RtpGetAmendableProjectsRequest request)
        {
            var results = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                // TODO: shouldn't need cycleId
                var availableProjects = RtpRepository.GetAmendableProjects(request.rtpPlanYearId, request.cycleId, true, true).ToList();
                availableProjects.ForEach(x => { results.Add(new System.Web.Mvc.SelectListItem { Text = x.Cycle.Name + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpGetAmendableProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }

            return results;
        }


        [HttpPost]
        public IEnumerable<System.Web.Mvc.SelectListItem> RtpGetAvailableRestoreProjects(RtpGetAmendableProjectsRequest request)
        {
            var results = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                var availableProjects = RtpRepository.GetRestoreProjectList(request.cycleId).ToList();
                availableProjects.ForEach(x => { results.Add(new System.Web.Mvc.SelectListItem { Text = x.RtpYear + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpGetAmendableProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }

            return results;
        }

        [HttpPost]
        public IEnumerable<DRCOG.Domain.Models.RTP.RtpSummary> RtpGetAmendablePendingProjects(RtpGetAmendableProjectsRequest request)
        {
            try
            {
                return RtpRepository.GetAmendableProjects(request.rtpPlanYearId, request.cycleId, false);
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpGetAmendableProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        [HttpPost]
        public IEnumerable<System.Web.Mvc.SelectListItem> RtpGetSponsorOrganizations(RtpGetAmendableProjectsRequest request)
        {
            try
            {
                var orgs = TripsRepository.RtpGetSponsorOrganizations(request.rtpPlanYearId);
                return orgs;
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpGetSponsorOrganizations failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        public class RtpAmendProjectsRequest
        {
            public int rtpPlanYearId;
            public int[] projectIds;
        }

        [HttpPost]
        public void RtpAmendProjects(RtpAmendProjectsRequest request)
        {
            try
            {
                TripsRepository.RtpAmendProjects(request.rtpPlanYearId, request.projectIds);
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpAmendProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        [HttpPost]
        public void RtpAdoptProjects(RtpAmendProjectsRequest request)
        {
            try
            {
                TripsRepository.RtpAdoptProjects(request.rtpPlanYearId, request.projectIds);
                // n.b. this sets the Cycle Status also
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpAdoptProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        [HttpPost]
        public void RtpRestoreProjects(RtpAmendProjectsRequest request)
        {
            try
            {
                TripsRepository.RtpRestoreProjects(request.rtpPlanYearId, request.projectIds);
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpRestoreProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        [HttpPost]
        public IEnumerable<System.Web.Mvc.SelectListItem> SurveyGetAmendableProjects()
        {
            var results = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                var availableProjects = SurveyRepository.GetAmendableProjects().ToList();
                availableProjects.ForEach(x => { results.Add(new System.Web.Mvc.SelectListItem { Text = x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                Logger.WarnException("SurveyGetAmendableProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }

            return results;
        }

        public class SurveyAmendProjectsRequest
        {
            public int surveyId;
            public int[] projectIds;
        }
        [HttpPost]
        public void SurveyAmendProjects(SurveyAmendProjectsRequest request)
        {
            try
            {
                TripsRepository.SurveyAmendProjects(request.surveyId, request.projectIds);
            }
            catch (Exception ex)
            {
                Logger.WarnException("RtpAmendProjects failed", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
