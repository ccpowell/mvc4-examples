using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ombudsman.Site.Controllers
{
    [Authorize]
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class HomeController : Controller
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public ActionResult IndexJq()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var facilities = repo.GetFacilityTypes();
            ViewBag.FacilityTypeId = new SelectList(facilities, "FacilityTypeId", "Name");
            ViewBag.IsManager = System.Web.Security.Roles.IsUserInRole("Manager");

            return View();
        }

        [AllowAnonymous]
        public ActionResult Test()
        {
            return View();
        }
    }
}
