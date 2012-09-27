using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace Ombudsman.Site.Controllers.Api
{
    [Authorize]
    public class FacilityController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        // GET api/facility
        public IEnumerable<Ombudsman.Models.Facility> Get()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            return repo.GetFacilities();
        }

        // GET api/facility/5
        public Ombudsman.Models.Facility Get(int id)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            return repo.GetFacility(id);
        }

        // POST api/facility
        [Authorize(Roles="Manager")]
        public void Post(Ombudsman.Models.Facility facility)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            facility.OmbudsmanId = repo.GetOmbudsmanIdFromName(facility.OmbudsmanName);
            repo.CreateFacility(facility);
        }

        // PUT api/facility/5
        public void Put(int id, Ombudsman.Models.Facility facility)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            facility.OmbudsmanId = repo.GetOmbudsmanIdFromName(facility.OmbudsmanName);
            var result = repo.UpdateFacility(facility, System.Web.Security.Roles.IsUserInRole("Manager"));
            if (!result)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }

        // DELETE api/facility/5
        public void Delete(int id)
        {
            throw new NotImplementedException("Delete of a Facility is not allowed.");
        }
    }
}
