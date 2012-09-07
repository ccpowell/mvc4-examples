using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TIPProjectVersion
    {
        public int TIPProjectVersionID { get; set; }
        public Nullable<System.DateTime> DeferralDate { get; set; }
        public Nullable<int> DeferralStatusID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<short> TimePeriodID { get; set; }
        public Nullable<int> VersionStatusID { get; set; }
        public Nullable<short> EndConstructionYear { get; set; }
        public string TIPID { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual Status Status { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
    }
}
