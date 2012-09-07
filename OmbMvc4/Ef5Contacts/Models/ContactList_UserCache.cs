using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_UserCache
    {
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Organization { get; set; }
        public string Title { get; set; }
        public string PrimaryContact { get; set; }
        public string LoweredEmail { get; set; }
        public string LoweredBusinessEmail { get; set; }
        public string LoweredApplicationName { get; set; }
    }
}
