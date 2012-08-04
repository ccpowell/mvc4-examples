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
    public class OmbController : Controller
    {
        private OmbMpContext db = new OmbMpContext();

        //
        // GET: /Omb/

        public ActionResult Index()
        {
            return View(db.Ombudsmen.ToList());
        }

        //
        // GET: /Omb/Details/5

        public ActionResult Details(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Find(id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // GET: /Omb/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Omb/Create

        [HttpPost]
        public ActionResult Create(Ombudsman ombudsman)
        {
            if (ModelState.IsValid)
            {
                db.Ombudsmen.Add(ombudsman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ombudsman);
        }

        //
        // GET: /Omb/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Find(id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // POST: /Omb/Edit/5

        [HttpPost]
        public ActionResult Edit(Ombudsman ombudsman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ombudsman).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ombudsman);
        }

        //
        // GET: /Omb/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Find(id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // POST: /Omb/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ombudsman ombudsman = db.Ombudsmen.Find(id);
            db.Ombudsmen.Remove(ombudsman);
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