using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_AttributeType
    {
        public ContactList_AttributeType()
        {
            this.ContactList_Attribute = new List<ContactList_Attribute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual ICollection<ContactList_Attribute> ContactList_Attribute { get; set; }
    }
}
