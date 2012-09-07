using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OmbMf.Models;
using System.Data.Entity.Migrations;

namespace OmbMf.Controllers.Api
{
    public class FacilityController : ApiController
    {
        private OmbMf.Models.OmbudsmanDbContext db = new OmbudsmanDbContext();

        // GET api/Facility
        [Queryable]
        public IQueryable<Facility> GetFacilities()
        {
            var facilities = db.Facilities.Include("Ombudsman");
            return facilities.AsQueryable();
        }

        // GET api/Facility/5
        public HttpResponseMessage GetFacility(int id)
        {
            Facility facility = db.Facilities.SingleOrDefault(f => f.FacilityId == id);
            if (facility == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, facility);
        }

        // PUT api/Facility/5
        public HttpResponseMessage PutFacility(int id, Facility facility)
        {
            if (ModelState.IsValid && id == facility.FacilityId)
            {
                db.Facilities.AddOrUpdate(facility);

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
                db.Facilities.AddOrUpdate(facility);
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
            var facility = db.Facilities.Single(f => f.FacilityId == id);
            if (facility == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Facilities.Remove(facility);

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