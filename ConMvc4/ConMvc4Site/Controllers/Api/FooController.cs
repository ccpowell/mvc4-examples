using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ConMvc4Site.Controllers.Api
{
    public class FooController : ApiController
    {
        // GET api/foo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/foo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/foo
        public void Post(string value)
        {
        }

        // PUT api/foo/5
        public void Put(int id, string value)
        {
        }

        // DELETE api/foo/5
        public void Delete(int id)
        {
        }
    }
}
