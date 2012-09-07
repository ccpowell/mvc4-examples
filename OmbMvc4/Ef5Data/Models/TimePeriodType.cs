using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TimePeriodType
    {
        public TimePeriodType()
        {
            this.TimePeriods = new List<TimePeriod>();
        }

        public int TimePeriodTypeID { get; set; }
        public string TimePeriodType1 { get; set; }
        public virtual ICollection<TimePeriod> TimePeriods { get; set; }
    }
}
