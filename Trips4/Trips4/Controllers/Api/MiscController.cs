using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Hosting;

namespace Trips4.Controllers.Api
{
    public class MiscController : ApiController
    {
        [AllowAnonymous]
        public string GetBuildInfo()
        {
            return System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Content/build.txt"));
        }
    }
}
