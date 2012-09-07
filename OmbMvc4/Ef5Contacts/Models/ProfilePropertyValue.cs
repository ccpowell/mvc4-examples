using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ProfilePropertyValue
    {
        public int ProfilePropertyValueID { get; set; }
        public int ProfilePropertyID { get; set; }
        public System.Guid UserID { get; set; }
        public string ProfilePropertyValue1 { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ProfileProperty ProfileProperty { get; set; }
    }
}
