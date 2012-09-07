using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class StatusType
    {
        public StatusType()
        {
            this.ContactList_Status = new List<ContactList_Status>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ContactList_Status> ContactList_Status { get; set; }
    }
}
