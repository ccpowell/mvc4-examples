using System;
using System.Web.Mvc;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.ViewModels.TIPProject;
using DRCOG.Domain.ViewModels;
//using DRCOG.Common.Web.MvcSupport.Attributes;

namespace Trips4.Controllers
{
    //[RemoteRequireHttps]
    public class ErrorController : ControllerBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorController()
        {
        }

        public ActionResult Index()
        {
            ErrorViewModel model = new ErrorViewModel("An error occurred while trying to process your request.");
            return View("~/Views/Shared/Error.aspx", model);
        }

        public ActionResult UnAuthorized(string resource, string message)
        {
            //Logger.Log.Warn("Unauthorized attempt to access " + resource + " by " + User.Identity.Name);
            var viewModel = new UnauthorizedViewModel
            {
                Message = message
                //CurrentUser = IsValidatedSession() ? GetSession().CurrentUser : null
            };
            
            return View(viewModel);
        }

    }
}
