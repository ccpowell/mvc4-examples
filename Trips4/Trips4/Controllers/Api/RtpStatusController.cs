using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;

namespace Trips4.Controllers.Api
{
    public class RtpStatusController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpRepository RtpRepository { get; set; }

        public RtpStatusController(Trips4.Data.TripsRepository trepo,
            IRtpRepository rrepo)
        {
            TripsRepository = trepo;
            RtpRepository = rrepo;
        }

        [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public void Put(DRCOG.Domain.Models.RTP.RtpStatusModel model)
        {
            try
            {
                RtpRepository.UpdateRtpStatus(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update TIP Status", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
