using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OmbMp.Models;

namespace OmbMp.Controllers
{
    public class FacController : Controller
    {
        private OmbMpContext db = new OmbMpContext();

        //
        // GET: /Fac/

        public ActionResult Index()
        {
            var facilities = db.Facilities.Include(f => f.Ombudsman);
            return View(facilities.ToList());
        }

        //
        // GET: /Fac/Details/5

        public ActionResult Details(int id = 0)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        //
        // GET: /Fac/Create

        public ActionResult Create()
        {
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name");
            return View();
        }

        //
        // POST: /Fac/Create

        [HttpPost]
        public ActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // GET: /Fac/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // POST: /Fac/Edit/5

        [HttpPost]
        public ActionResult Edit(Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OmbudsmanId = new SelectList(db.Ombudsmen, "OmbudsmanId", "Name", facility.OmbudsmanId);
            return View(facility);
        }

        //
        // GET: /Fac/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        //
        // POST: /Fac/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Facility facility = db.Facilities.Find(id);
            db.Facilities.Remove(facility);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}