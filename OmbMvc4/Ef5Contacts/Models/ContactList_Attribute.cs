using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_Attribute
    {
        public ContactList_Attribute()
        {
            this.ContactList_ListAttribute = new List<ContactList_ListAttribute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int AttributeTypeId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual ContactList_AttributeType ContactList_AttributeType { get; set; }
        public virtual ICollection<ContactList_ListAttribute> ContactList_ListAttribute { get; set; }
    }
}
