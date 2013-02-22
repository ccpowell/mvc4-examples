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
    [AuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
    public class RtpProjectSegmentController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpProjectRepository RtpProjectRepository { get; set; }

        public RtpProjectSegmentController(IRtpProjectRepository rprepo,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            RtpProjectRepository = rprepo;
        }

        public SegmentModel Get(int id)
        {
            SegmentModel model;
            try
            {
                model = RtpProjectRepository.GetSegmentDetails(id);
                model.LRSSchemeBase = RtpProjectRepository.GetLRSScheme((int)SchemeName.LRSProjects);
                var xml = new DRCOG.TIP.Services.XMLService(RtpProjectRepository);
                model._LRS = xml.LoadRecords((int)SchemeName.LRSProjects, id); 
                return model;
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Get RTP Project Segment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public void Delete(int id)
        {
            try
            {
                RtpProjectRepository.DeleteSegment(id);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not Delete RTP Project Segment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public void Put(SegmentModel model)
        {
            try
            {
                RtpProjectRepository.UpdateSegment(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Project Segment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }

        public void Post(SegmentModel model)
        {
            //Send update to repo
            try
            {
                RtpProjectRepository.AddSegment(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not create RTP Project Segment", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
