﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OmbMf.Models;

namespace OmbMf.Controllers.Api
{
    public class FacilityController : ApiController
    {
        private OmbudsmanEntities db = new OmbudsmanEntities();

        // GET api/Facility
        [Queryable]
        public IQueryable<Facility> GetFacilities()
        {
            var facilities = db.Facilities.Include("Ombudsman");
            return facilities.AsQueryable();
        }

        // GET api/Facility/5
        public Facility GetFacility(int id)
        {
            Facility facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return facility;
        }

        // PUT api/Facility/5
        public HttpResponseMessage PutFacility(int id, Facility facility)
        {
            if (ModelState.IsValid && id == facility.FacilityId)
            {
                db.Facilities.Attach(facility);
                db.ObjectStateManager.ChangeObjectState(facility, EntityState.Modified);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, facility);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Facility
        public HttpResponseMessage PostFacility(Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.AddObject(facility);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, facility);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = facility.FacilityId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Facility/5
        public HttpResponseMessage DeleteFacility(int id)
        {
            Facility facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Facilities.DeleteObject(facility);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, facility);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}