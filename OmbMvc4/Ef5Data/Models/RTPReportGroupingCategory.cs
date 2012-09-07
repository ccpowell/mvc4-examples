using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RTPReportGroupingCategory
    {
        public int RTPReportGroupingCategoryID { get; set; }
        public Nullable<short> TimePeriodID { get; set; }
        public string ShortTitle { get; set; }
        public Nullable<bool> Subtotals { get; set; }
        public Nullable<bool> ListDisplay { get; set; }
        public virtual Category Category { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
    }
}
