using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class aspnet_PersonalizationAllUsers
    {
        public System.Guid PathId { get; set; }
        public byte[] PageSettings { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public virtual aspnet_Paths aspnet_Paths { get; set; }
    }
}
