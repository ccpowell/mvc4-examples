using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ContactsMvc4.Controllers
{
    public class ContactList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ListController : ApiController
    {

        // GET api/List
        public IEnumerable<ContactList> GetContactLists()
        {
            var result = db.ContactLists.AsEnumerable();
            return result;
        }

        // GET api/List/5
        public ContactListSummary GetContactList(int id)
        {
            ContactList contactlist = db.ContactLists.Find(id);
            if (contactlist == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return new ContactListSummary()
                {
                    Id = contactlist.Id,
                    Name = contactlist.Name
                };
        }

        // PUT api/List/5
        public HttpResponseMessage PutContactList(int id, ContactList contactlist)
        {
            if (ModelState.IsValid && id == contactlist.Id)
            {
                db.Entry(contactlist).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, contactlist);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/List
        public HttpResponseMessage PostContactList(ContactList contactlist)
        {
            if (ModelState.IsValid)
            {
                db.ContactLists.Add(contactlist);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, contactlist);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = contactlist.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/List/5
        public HttpResponseMessage DeleteContactList(int id)
        {
            ContactList contactlist = db.ContactLists.Find(id);
            if (contactlist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ContactLists.Remove(contactlist);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, contactlist);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}