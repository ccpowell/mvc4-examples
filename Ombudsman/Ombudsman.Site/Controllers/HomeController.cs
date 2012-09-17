using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ombudsman.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var ombudsmen = repo.GetOmbudsmen();
            var facilities = repo.GetFacilityTypes();
            ViewBag.FacilityTypeId = new SelectList(facilities, "FacilityTypeId", "Name");
            ViewBag.OmbudsmanId = new SelectList(ombudsmen, "OmbudsmanId", "Name");

            return View();
        }

    }
}
