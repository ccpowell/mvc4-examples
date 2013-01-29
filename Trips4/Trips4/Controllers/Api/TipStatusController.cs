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
    [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
    public class TipStatusController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        public TipStatusController(Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
        }

        public void Put(DRCOG.Domain.Models.TipStatusModel model)
        {
            try
            {
                TripsRepository.UpdateTipStatus(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update TIP Status", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
