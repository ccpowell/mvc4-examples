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
    public class SurveyLocationController : ApiController
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private ISurveyRepository SurveyRepository { get; set; }

        public SurveyLocationController(ISurveyRepository surveyRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            SurveyRepository = surveyRepository;
        }
        
        public void Put(FormDataCollection form)
        {
            // Manually parse up the form b/c of the muni & county split stuff
            // TODO: make a custom formatter
            int projectVersionId = Convert.ToInt32(form.Get("ProjectVersionId"));
            string year = form.Get("Year");
            //Get the existing model from the datagbase
            LocationModel model = SurveyRepository.GetProjectLocationModel(projectVersionId, year);
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
                SurveyRepository.UpdateProjectLocationModel(model, projectVersionId);
                SurveyRepository.CheckUpdateStatusId(SurveyRepository.GetProjectBasics(projectVersionId));
                //Update the county shares
                foreach (CountyShareModel m in countyShares.Values)
                {
                    SurveyRepository.UpdateCountyShare(m);
                }
                //Update the muni shares
                foreach (MunicipalityShareModel m in muniShares.Values)
                {
                    SurveyRepository.UpdateMunicipalityShare(m);
                }
                //Ok, we're good.
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update Survey Location", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
