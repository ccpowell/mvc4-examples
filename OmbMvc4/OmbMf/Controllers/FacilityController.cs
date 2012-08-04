using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmbMf.Models;
using System.Diagnostics;

namespace OmbMf.Controllers
{
    public class FacilityController : Controller
    {
        private OmbudsmanEntities db = new OmbudsmanEntities();

        //
        // GET: /Facility/

        public ActionResult Index()
        {
            var facilities = db.Facilities.Include("Ombudsman");
            return View(facilities.ToList());
        }

        //
        // GET: /Facility/Details/5

        public ActionResult Details(int id = 0)
        {
            Facility facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        //
        // GET: /Facility/Create

        public ActionResult Create()
        {
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name");
            return View();
        }

        //
        // POST: /Facility/Create

        [HttpPost]
        public ActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.AddObject(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // GET: /Facility/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Facility facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // POST: /Facility/Edit/5

        [HttpPost]
        public ActionResult Edit(Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.Attach(facility);
                db.ObjectStateManager.ChangeObjectState(facility, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // GET: /Facility/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Facility facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        //
        // POST: /Facility/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Facility facility = db.Facilities.Single(f => f.FacilityId == id);
                db.Facilities.DeleteObject(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            ModelState.AddModelError("FacilityId", "This facility cannot be deleted.");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}