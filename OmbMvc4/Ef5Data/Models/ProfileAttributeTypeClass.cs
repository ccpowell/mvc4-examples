using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProfileAttributeTypeClass
    {
        public ProfileAttributeTypeClass()
        {
            this.ProfileAttributeTypes = new List<ProfileAttributeType>();
        }

        public System.Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public virtual ICollection<ProfileAttributeType> ProfileAttributeTypes { get; set; }
    }
}
