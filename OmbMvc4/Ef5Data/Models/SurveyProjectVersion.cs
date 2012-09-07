using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class SurveyProjectVersion
    {
        public int SurveyProjectVersionID { get; set; }
        public Nullable<int> UpdateStatusID { get; set; }
        public Nullable<int> VersionStatusID { get; set; }
        public Nullable<short> TimePeriodID { get; set; }
        public Nullable<int> ActionStatusID { get; set; }
        public Nullable<short> EndConstructionYear { get; set; }
        public Nullable<decimal> ConstantCost { get; set; }
        public Nullable<decimal> VisionCost { get; set; }
        public Nullable<decimal> AmendedCost { get; set; }
        public Nullable<decimal> YOECost { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual Status Status { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual Status Status2 { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
    }
}
