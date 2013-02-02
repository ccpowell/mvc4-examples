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
    [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
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
        public void Put(LocationModel model)
        {
            try
            {
                ProjectRepository.UpdateProjectLocationModel(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update TIP Project Location", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
