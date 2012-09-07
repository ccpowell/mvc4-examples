using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList
    {
        public ContactList()
        {
            this.ContactList_ListAttribute = new List<ContactList_ListAttribute>();
            this.ContactList_Member = new List<ContactList_Member>();
            this.ContactList_Owner = new List<ContactList_Owner>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime LastModified { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual ICollection<ContactList_ListAttribute> ContactList_ListAttribute { get; set; }
        public virtual ICollection<ContactList_Member> ContactList_Member { get; set; }
        public virtual ICollection<ContactList_Owner> ContactList_Owner { get; set; }
    }
}
