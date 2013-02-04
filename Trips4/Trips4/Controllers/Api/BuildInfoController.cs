using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Hosting;

namespace Trips4.Controllers.Api
{
    public class BuildInfoController : ApiController
    {
        private static readonly string BuildInfo = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Content/build.txt"));
        [AllowAnonymous]
        public string Get()
        {
            return BuildInfo;
        }
    }
}
