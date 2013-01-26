using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contacts.Controllers.Operation
{
    public class MiscController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IEnumerable<Models.Contact> GetAutoCompleteContact(string field, string prefix)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    return repo.GetAutoCompleteContact(field, prefix);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("GetAutoCompleteContact failed", ex);
                throw;
            }
        }

        public class AddContactRequest
        {
            public string contactlist { get; set; }
            public string contact { get; set; }
        }

        // N.B. Web Api post methods require single arguments
        [HttpPost]
        public void AddContact(AddContactRequest request)
        {
            try
            {
                using (var repo = new Data.ContactsRepository())
                {
                    var clist = repo.GetContactList(request.contactlist);
                    if (!clist.ContactIds.Contains(request.contact))
                    {
                        clist.ContactIds.Add(request.contact);
                    }
                    repo.UpdateContactList(clist);
                }
            }
            catch (Exception ex)
            {
                Logger.DebugException("AddContact failed", ex);
                throw;
            }
        }

        [HttpGet]
        public string GetX(string foo)
        {
            return foo ?? "not specified";
        }
    }
}
