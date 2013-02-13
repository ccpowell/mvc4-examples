using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Common.Services;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using Trips4.Configuration;
using Trips4.Data;

namespace Trips4.Controllers.Operation
{
    [AuthorizeAttribute(Roles = "Administrator, RTP Administrator")]
    public class RtpOperationController : ApiController
    {
        private readonly IRtpRepository RtpRepository;
        private readonly IRtpProjectRepository RtpProjectRepository;
        private readonly TripsRepository TripsRepository;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public RtpOperationController(IRtpRepository rtpRepo,
            IRtpProjectRepository projectRepo, TripsRepository trepo)
        {
            RtpRepository = rtpRepo;
            RtpProjectRepository = projectRepo;
            TripsRepository = trepo;
        }

        public class Message { public string message { get; set; } }
        [HttpPost]
        public string Hello(Message message)
        {
            return message.message;
        }


        [HttpPost]
        public int CreatePlan(Trips4.Data.TripsRepository.RtpCreatePlanRequest request)
        {
            return TripsRepository.RtpCreatePlan(request);
        }
    }
}
