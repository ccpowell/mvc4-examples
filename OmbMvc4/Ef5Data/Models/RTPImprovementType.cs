using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RTPImprovementType
    {
        public int RTPImprovementTypeID { get; set; }
        public string Subcode { get; set; }
        public string MajorCode { get; set; }
        public string Code { get; set; }
        public virtual ImprovementType ImprovementType { get; set; }
    }
}
