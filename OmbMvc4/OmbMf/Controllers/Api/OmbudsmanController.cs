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

namespace OmbMf.Controllers.Api
{
    public class OmbudsmanController : ApiController
    {
        private OmbudsmanEntities db = new OmbudsmanEntities();

        // GET api/Ombudsman
        public IEnumerable<Ombudsman> GetOmbudsmen()
        {
            return db.Ombudsmen.AsEnumerable();
        }

        // GET api/Ombudsman/5
        public Ombudsman GetOmbudsman(int id)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            if (ombudsman == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return ombudsman;
        }

        // PUT api/Ombudsman/5
        public HttpResponseMessage PutOmbudsman(int id, Ombudsman ombudsman)
        {
            if (ModelState.IsValid && id == ombudsman.OmbudsmanId)
            {
                db.Ombudsmen.Attach(ombudsman);
                db.ObjectStateManager.ChangeObjectState(ombudsman, EntityState.Modified);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, ombudsman);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Ombudsman
        public HttpResponseMessage PostOmbudsman(Ombudsman ombudsman)
        {
            if (ModelState.IsValid)
            {
                db.Ombudsmen.AddObject(ombudsman);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, ombudsman);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = ombudsman.OmbudsmanId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Ombudsman/5
        public HttpResponseMessage DeleteOmbudsman(int id)
        {
            Ombudsman ombudsman = db.Ombudsmen.Single(o => o.OmbudsmanId == id);
            if (ombudsman == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Ombudsmen.DeleteObject(ombudsman);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ombudsman);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}