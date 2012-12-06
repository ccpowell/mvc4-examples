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

        public MiscController(Trips4.Data.TripsRepository trepo, IRtpRepository rrepo)
        {
            TripsRepository = trepo;
            RtpRepository = rrepo;
        }

        [HttpGet]
        public string Hello()
        {
            try
            {
                return "hello";
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not say hello", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public class Stuff
        {
            public string Name { get; set; }
            public List<int> Ints { get; set; }
        }


        [HttpPost]
        public Stuff PostStuff(Stuff stuff)
        {
            return stuff;
        }


        public class RtpGetAmendableProjectsRequest
        {
            public int cycleId;
            public int rtpPlanYearId;
        }

        [HttpPost]
        public List<System.Web.Mvc.SelectListItem> RtpGetAmendableProjects(RtpGetAmendableProjectsRequest request)
        {
            var results = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                var availableProjects = RtpRepository.GetAmendableProjects(request.rtpPlanYearId, request.cycleId, true, true).ToList();
                availableProjects.ForEach(x => { results.Add(new System.Web.Mvc.SelectListItem { Text = x.Cycle.Name + ": " + x.ProjectName, Value = x.ProjectVersionId.ToString() }); });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }

            return results;
        }

        public class RtpAmendProjectsRequest
        {
            public int rtpPlanYearId;
            public int[] projectIds;
        }

        [HttpPost]
        public void RtpAmendProjects(RtpAmendProjectsRequest request)
        {
        }
    }
}
