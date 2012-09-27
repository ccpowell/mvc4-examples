using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ombudsman.Site.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("IndexJq");
        }

        public ActionResult IndexJq()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var facilities = repo.GetFacilityTypes();
            ViewBag.FacilityTypeId = new SelectList(facilities, "FacilityTypeId", "Name");

            //var ombudsmen = repo.GetOmbudsmen();
            //ViewBag.OmbudsmanId = new SelectList(ombudsmen, "OmbudsmanId", "Name");

            ViewBag.IsManager = System.Web.Security.Roles.IsUserInRole("Manager");

            return View();
        }


    }
}
