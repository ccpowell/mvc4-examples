using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_ListAttribute
    {
        public int ContactListId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
        public virtual ContactList ContactList { get; set; }
        public virtual ContactList_Attribute ContactList_Attribute { get; set; }
    }
}
