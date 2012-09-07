using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class StatusType
    {
        public int StatusTypeID { get; set; }
        public string StatusType1 { get; set; }
        public Nullable<int> ParentStatusTypeID { get; set; }
    }
}
