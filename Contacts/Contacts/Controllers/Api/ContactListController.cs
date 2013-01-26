using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contacts.Controllers.Api
{
    public class ContactListController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        // GET api/contactlist
        public IEnumerable<Models.ContactList> Get()
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    return repo.GetContactLists();
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Get failed", ex);
                throw;
            }
        }

        // GET api/contactlist/xxx
        public Models.ContactList Get(string id)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    return repo.GetContactList(id);
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
        public void Post(Models.ContactList contact)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.CreateContactList(contact);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Post failed", ex);
                throw;
            }
        }

        // PUT api/contact
        [HttpPut]
        public void Put(Models.ContactList contact)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.UpdateContactList(contact);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Put failed", ex);
                throw;
            }
        }

        // DELETE api/contact/5
        [HttpDelete]
        public void Delete(string id)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    repo.DeleteContactList(id);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("Delete failed", ex);
                throw;
            }
        }
    }
}
