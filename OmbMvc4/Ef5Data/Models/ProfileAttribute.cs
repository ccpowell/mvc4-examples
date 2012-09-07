using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProfileAttribute
    {
        public System.Guid AttributeId { get; set; }
        public int ProfileId { get; set; }
        public string AttributeValue { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ProfileAttributeType ProfileAttributeType { get; set; }
    }
}
