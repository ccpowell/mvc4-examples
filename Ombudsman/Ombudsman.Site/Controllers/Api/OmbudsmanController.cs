using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ombudsman.Site.Controllers.Api
{
    public class OmbudsmanController : ApiController
    {
        // GET api/ombudsman
        public IEnumerable<Ombudsman.Models.Ombudsman> Get()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            return repo.GetOmbudsmen();
        }

        // GET api/ombudsman/5
        public Ombudsman.Models.Ombudsman Get(int id)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            return repo.GetOmbudsman(id);
        }

        // POST api/ombudsman
        public void Post(Ombudsman.Models.Ombudsman value)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            repo.CreateOmbudsman(value);
        }

        // PUT api/ombudsman/5
        public void Put(int id, Ombudsman.Models.Ombudsman value)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            repo.UpdateOmbudsman(value);
        }

        // DELETE api/ombudsman/5
        public void Delete(int id)
        {
            throw new NotImplementedException("Delete of an Ombudsman is not allowed.");
        }
    }
}
