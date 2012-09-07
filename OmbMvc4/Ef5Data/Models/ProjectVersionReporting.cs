using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectVersionReporting
    {
        public int ProjectVersionID { get; set; }
        public Nullable<short> AmendmentOrder { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public Nullable<int> TimePeriodID { get; set; }
    }
}
