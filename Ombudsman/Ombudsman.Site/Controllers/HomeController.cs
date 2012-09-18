﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ombudsman.Site.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
#if knockout
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var ombudsmen = repo.GetOmbudsmen();
            var facilities = repo.GetFacilityTypes();
            ViewBag.FacilityTypeId = new SelectList(facilities, "FacilityTypeId", "Name");
            ViewBag.OmbudsmanId = new SelectList(ombudsmen, "OmbudsmanId", "Name");

            return View();
#endif
            return RedirectToActionPermanent("IndexJq");
        }

        public ActionResult IndexJq()
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
