using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ContactList_MemberAudit
    {
        public int Id { get; set; }
        public int ContactList_Id { get; set; }
        public System.Guid UserId { get; set; }
        public System.Guid ApproverUserId { get; set; }
        public int ContactList_Status_Id { get; set; }
        public string Comment { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
