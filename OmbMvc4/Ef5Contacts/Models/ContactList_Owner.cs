using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_Owner
    {
        public int ContactList_Id { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<int> ContactList_Status_Id { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ContactList ContactList { get; set; }
        public virtual ContactList_Status ContactList_Status { get; set; }
    }
}
