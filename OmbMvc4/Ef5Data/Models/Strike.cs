using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Strike
    {
        public int StrikeID { get; set; }
        public int ProjectID { get; set; }
        public Nullable<int> StrikeReasonID { get; set; }
        public string SponsorReaction { get; set; }
        public string DRCOGAction { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public int StatusID { get; set; }
        public string StatusOther { get; set; }
        public virtual Category Category { get; set; }
        public virtual Project Project { get; set; }
        public virtual Status Status { get; set; }
    }
}
