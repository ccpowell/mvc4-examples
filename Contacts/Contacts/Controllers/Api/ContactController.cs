using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contacts.Controllers.Api
{
    public class ContactController : ApiController
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        // GET api/contact
        public IEnumerable<Models.Contact> Get()
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    return repo.GetContacts();
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Get failed", ex);
                throw;
            }
        }

        // GET api/contact/
        public Models.Contact Get(string id)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    return repo.GetContact(id);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Get failed", ex);
                throw;
            }
        }

        // POST api/contact
        [HttpPost]
        public void Post(Models.Contact contact)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.CreateContact(contact);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Post failed", ex);
                throw;
            }
        }

        // PUT api/contact/5
        [HttpPut]
        public void Put(Models.Contact contact)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.UpdateContact(contact);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Put failed", ex);
                throw;
            }
        }

        // DELETE api/contact/5
        public void Delete(string id)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.DeleteContact(id);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Put failed", ex);
                throw;
            }
        }
    }
}
