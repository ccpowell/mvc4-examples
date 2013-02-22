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
using DRCOG.Domain.Helpers;
using DRCOG.Domain.ViewModels.RTP.Project;

namespace Trips4.Controllers.Api
{
    //[AuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
    public class RtpProjectLrsController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpProjectRepository RtpProjectRepository { get; set; }

        public RtpProjectLrsController(IRtpProjectRepository rprepo,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            RtpProjectRepository = rprepo;
        }

        public class Query
        {
            public int? SegmentId { get; set; }
        }

        public IEnumerable<LRS> Get(int segmentId, int x)
        {
            try
            {
                return TripsRepository.RtpGetLrsForSegment(segmentId);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Get RTP Project LRS", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        public LRS Get(int id)
        {
            try
            {
                return TripsRepository.RtpGetLrs(id);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Get RTP Project LRS", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public void Delete(int id)
        {
            try
            {
                TripsRepository.RtpDeleteLrs(id);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Delete RTP Project LRS", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public void Put(LRS model)
        {
            try
            {
                TripsRepository.RtpUpdateLrs(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Project LRS", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        public int Post(LRS model)
        {
            //Send update to repo
            try
            {
                return TripsRepository.RtpCreateLrs(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not create RTP Project LRS", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
