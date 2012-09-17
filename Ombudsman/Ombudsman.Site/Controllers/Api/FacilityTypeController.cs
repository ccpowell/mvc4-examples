using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace Ombudsman.Site.Controllers.Api
{
    public class FacilityTypeController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        // GET api/facilitytype
        public IEnumerable<Ombudsman.Models.FacilityType> Get()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            return repo.GetFacilityTypes();
        }

    }
}
