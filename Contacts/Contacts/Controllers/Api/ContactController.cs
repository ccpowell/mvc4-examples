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
                throw;
            }
        }
#if hoo
        // GET api/contact/5
        public string Get(string id)
        {
            return "value";
        }
#endif
        // GET api/contact/5
        public string Get(string id)
        {
            return "value";
        }

        // POST api/contact
        public void Post(string value)
        {
        }

        // PUT api/contact/5
        public void Put(int id, string value)
        {
        }

        // DELETE api/contact/5
        public void Delete(int id)
        {
        }
    }
}
