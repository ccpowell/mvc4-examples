using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Interfaces;

namespace Trips4.Controllers.Api
{
    /// <summary>
    /// This is the API controller for Rtp Projects. 
    /// The only operation is creating an RTP Project.
    /// The name avoids name conflicts with the existing RtpProjectController.
    /// </summary>
    public class RtpProjectItemController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IRtpRepository RtpRepository { get; set; }

        public RtpProjectItemController(IRtpRepository rrepo,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            RtpRepository = rrepo;
        }
        public class PostData
        {
            public string projectName { get; set; } 
            public string facilityName { get; set; } 
            public string plan { get; set; } 
            public int sponsorOrganizationId { get; set; } 
            public int? cycleId { get; set; }
        }

        // POST api/rtpproject
        public int Post(PostData project)
        {
            return RtpRepository.CreateProject(project.projectName, project.facilityName, project.plan, project.sponsorOrganizationId, project.cycleId);
        }
    }
}
