using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class ProfileProperty
    {
        public ProfileProperty()
        {
            this.ProfilePropertyValues = new List<ProfilePropertyValue>();
        }

        public int ProfilePropertyID { get; set; }
        public string ProfilePropertyName { get; set; }
        public string ProfilePropertyType { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public virtual ICollection<ProfilePropertyValue> ProfilePropertyValues { get; set; }
    }
}
