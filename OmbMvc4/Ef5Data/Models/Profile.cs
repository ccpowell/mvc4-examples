using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Profile
    {
        public Profile()
        {
            this.ProfileAttributes = new List<ProfileAttribute>();
        }

        public int ProfileID { get; set; }
        public int PersonID { get; set; }
        public int ProfileTypeID { get; set; }
        public virtual ICollection<ProfileAttribute> ProfileAttributes { get; set; }
    }
}
