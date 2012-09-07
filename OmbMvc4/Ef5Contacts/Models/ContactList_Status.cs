using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_Status
    {
        public ContactList_Status()
        {
            this.ContactList_Member = new List<ContactList_Member>();
            this.ContactList_Owner = new List<ContactList_Owner>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusTypeId { get; set; }
        public virtual ICollection<ContactList_Member> ContactList_Member { get; set; }
        public virtual ICollection<ContactList_Owner> ContactList_Owner { get; set; }
        public virtual StatusType StatusType { get; set; }
    }
}
