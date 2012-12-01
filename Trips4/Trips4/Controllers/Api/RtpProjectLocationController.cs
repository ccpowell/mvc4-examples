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

namespace Trips4.Controllers.Api
{
    public class RtpProjectLocationController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpProjectRepository RtpProjectRepository { get; set; }

        public RtpProjectLocationController(IRtpProjectRepository rprepo,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            RtpProjectRepository = rprepo;
        }

        [AuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
        public void Put(FormDataCollection form)
        {
            // Manually parse up the form b/c of the muni & county split stuff
            // TODO: make a custom formatter
            int projectVersionId = Convert.ToInt32(form.Get("ProjectVersionId"));
            string year = form.Get("RtpYear");
            //Get the existing model from the datagbase
            LocationModel model = RtpProjectRepository.GetProjectLocationModel(projectVersionId, year);
            //Update values
            model.Limits = form.Get("Limits");
            model.FacilityName = form.Get("FacilityName");
            int testOut = 0;
            Int32.TryParse(form.Get("RouteId"), out testOut);
            model.RouteId = testOut;
            
            //parse out the county & muni shares stuff... 
            var nvc = form.ReadAsNameValueCollection();
            Dictionary<int, CountyShareModel> countyShares = ControllerBase.ExtractCountyShares(nvc);
            Dictionary<int, MunicipalityShareModel> muniShares = ControllerBase.ExtractMuniShares(nvc);

            //Send updates to repo
            try
            {
                RtpProjectRepository.UpdateProjectLocationModel(model);
                //Update the county shares
                foreach (CountyShareModel m in countyShares.Values)
                {
                    RtpProjectRepository.UpdateCountyShare(m);
                }
                //Update the muni shares
                foreach (MunicipalityShareModel m in muniShares.Values)
                {
                    RtpProjectRepository.UpdateMunicipalityShare(m);
                }
                //Ok, we're good.
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update RTP Project Location", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
