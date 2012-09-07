using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProfileAttributeType
    {
        public ProfileAttributeType()
        {
            this.ProfileAttributes = new List<ProfileAttribute>();
        }

        public System.Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public System.Guid ClassId { get; set; }
        public virtual ICollection<ProfileAttribute> ProfileAttributes { get; set; }
        public virtual ProfileAttributeTypeClass ProfileAttributeTypeClass { get; set; }
    }
}
