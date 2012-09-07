using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Raw
    {
        public string ReportYear { get; set; }
        public string TipId { get; set; }
        public Nullable<double> NetObligation { get; set; }
        public Nullable<bool> BikePedElement { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    }
}
