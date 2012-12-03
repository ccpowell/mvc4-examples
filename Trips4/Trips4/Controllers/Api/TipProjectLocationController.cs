using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ViewModels.TIPProject;
using DRCOG.Domain.Models.TIPProject;
using System.Net.Http.Formatting;
using DRCOG.Domain.Models;

namespace Trips4.Controllers.Api
{
    public class TipProjectLocationController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IProjectRepository ProjectRepository { get; set; }

        public TipProjectLocationController(IProjectRepository projectRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            ProjectRepository = projectRepository;
        }

        [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public void Put(FormDataCollection form)
        {
            //Manually parse up the form b/c of the muni & county split stuff
            int projectVersionId = Convert.ToInt32(form.Get("ProjectVersionId"));
            string year = form.Get("TipYear");
            //Get the existing model from the datagbase
            LocationModel model = ProjectRepository.GetProjectLocationModel(projectVersionId, year);
            //Update values
            model.Limits = form.Get("Limits");
            model.FacilityName = form.Get("FacilityName");
            //model.CdotRegionId = 
            int cdotRegion = default(int);
            int delaysLocation = default(int);
            Int32.TryParse(form.Get("TipProjectLocation.CdotRegionId"), out cdotRegion);
            Int32.TryParse(form.Get("TipProjectLocation.AffectedProjectDelaysLocationId"), out delaysLocation);

            model.CdotRegionId = cdotRegion;
            model.AffectedProjectDelaysLocationId = delaysLocation;

            //parse out the county & muni shares stuff...
            var nvc = form.ReadAsNameValueCollection();
            Dictionary<int, CountyShareModel> countyShares = ControllerBase.ExtractCountyShares(nvc);
            Dictionary<int, MunicipalityShareModel> muniShares = ControllerBase.ExtractMuniShares(nvc);

            //Send updates to repo
            try
            {
                ProjectRepository.UpdateProjectLocationModel(model);
                //Update the county shares
                foreach (CountyShareModel m in countyShares.Values)
                {
                    ProjectRepository.UpdateCountyShare(m);
                }
                //Update the muni shares
                foreach (MunicipalityShareModel m in muniShares.Values)
                {
                    ProjectRepository.UpdateMunicipalityShare(m);
                }
                //Ok, we're good.
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update TIP Project Location", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
