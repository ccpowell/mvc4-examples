using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ViewModels.TIPProject;

namespace Trips4.Controllers.Api
{
    public class TipProjectScopeController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IProjectRepository ProjectRepository { get; set; }

        public TipProjectScopeController(IProjectRepository projectRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            ProjectRepository = projectRepository;
        }

        [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public void Put(ScopeViewModel viewModel)
        {
            //Send update to repo
            try
            {
                ProjectRepository.UpdateProjectScope(viewModel.TipProjectScope);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not TIP Project Scope", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
