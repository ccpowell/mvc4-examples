using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Trips4.Controllers.Operation
{
    public class MiscController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        public MiscController(Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
        }

        [HttpGet]
        public string Hello()
        {
            try
            {
                return "hello";
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not say hello", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }


        public class Stuff
        {
            public string Name { get; set; }
            public List<int> Ints { get; set; }
        }


        [HttpPost]
        public Stuff PostStuff(Stuff stuff)
        {
            return stuff;
        }
    }
}
