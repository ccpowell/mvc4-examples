using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TimePeriodCycle
    {
        public int CycleId { get; set; }
        public short TimePeriodId { get; set; }
        public Nullable<byte> ListOrder { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
    }
}
