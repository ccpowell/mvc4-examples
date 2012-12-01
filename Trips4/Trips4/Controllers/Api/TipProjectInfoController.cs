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
    public class TipProjectInfoController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        private IProjectRepository ProjectRepository { get; set; }

        public TipProjectInfoController(IProjectRepository projectRepository,
            Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
            ProjectRepository = projectRepository;
        }

        public void Put(InfoViewModel viewModel)
        {
            //Send update to repo
            try
            {
                //throw new Exception("fooey");
                ProjectRepository.UpdateProjectInfo(viewModel.InfoModel);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update Survey Info", ex);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed) { ReasonPhrase = ex.Message });
            }
        }
    }
}
