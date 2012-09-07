using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class Version
    {
        public System.Guid Uid { get; set; }
        public System.Guid SessionId { get; set; }
        public string PropertyName { get; set; }
        public string Prior { get; set; }
        public string Current { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
