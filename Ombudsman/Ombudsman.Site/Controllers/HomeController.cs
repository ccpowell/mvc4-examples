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

            //var ombudsmen = repo.GetOmbudsmen();
            //ViewBag.OmbudsmanId = new SelectList(ombudsmen, "OmbudsmanId", "Name");

            ViewBag.IsManager = System.Web.Security.Roles.IsUserInRole("Manager");
            ViewBag.IsNewSession = "Session is new? " + Session.IsNewSession;
            Response.SetCookie(new HttpCookie("hoover", Session.SessionID));
            Session["something"] = 42;

            Logger.Debug("Session is new = " + Session.IsNewSession);
            Logger.Debug("Session ID = " + Session.SessionID);
            Logger.Debug("Session cookie mode = " + Session.CookieMode);

            return View();
        }


    }
}
