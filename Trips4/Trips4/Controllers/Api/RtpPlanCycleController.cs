using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.ViewModels.RTP;

namespace Trips4.Controllers.Api
{
    public class RtpPlanCycleController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        public RtpPlanCycleController(Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
        }

        // GET api/rtpplancycle/5
        public PlanCycle Get(int id)
        {
            var found = TripsRepository.GetRtpPlanCycle(id);
            if (found == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "No such RTP Plan Cycle" });
            }
            return found;
        }

        // PUT api/rtpplancycle/5
        public void Put(PlanCycle cycle)
        {
            try
            {
                TripsRepository.UpdateRtpPlanCycle(cycle);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Plan Cycle", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        // POST api/rtpplancycle
        public int Post(PostData data)
        {
            var cycle = new PlanCycle() { Name = data.name, Description = data.description };
            try
            {
                return TripsRepository.CreateRtpPlanCycle(cycle, data.rtpYearId);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not create RTP Plan Cycle", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        // only one item can come from the body, so we put the plan year together with the cycle data
        public class PostData
        {
            public string name { get; set; }
            public string description { get; set; }
            public int rtpYearId { get; set; }
        }
    }
}
