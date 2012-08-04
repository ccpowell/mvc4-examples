﻿using System;
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
    public class OmbudsmanController : Controller
    {
        private OmbudsmanEntities db = new OmbudsmanEntities();

        //
        // GET: /Ombudsman/

        public ActionResult Index()
        {
            return View(db.Ombudsmen.ToList());
        }

        //
        // GET: /Ombudsman/Details/5

        public ActionResult Details(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // GET: /Ombudsman/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Ombudsman/Create

        [HttpPost]
        public ActionResult Create(Ombudsman ombudsman)
        {
            if (ModelState.IsValid)
            {
                db.Ombudsmen.AddObject(ombudsman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ombudsman);
        }

        //
        // GET: /Ombudsman/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // POST: /Ombudsman/Edit/5

        [HttpPost]
        public ActionResult Edit(Ombudsman ombudsman)
        {
            if (ModelState.IsValid)
            {
                db.Ombudsmen.Attach(ombudsman);
                db.ObjectStateManager.ChangeObjectState(ombudsman, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ombudsman);
        }

        //
        // GET: /Ombudsman/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            if (ombudsman == null)
            {
                return HttpNotFound();
            }
            return View(ombudsman);
        }

        //
        // POST: /Ombudsman/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            try
            {
                db.Ombudsmen.DeleteObject(ombudsman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            ModelState.AddModelError("", "This ombudsman cannot be deleted.");
            return View(ombudsman);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}