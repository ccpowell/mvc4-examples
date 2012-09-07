using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingTypeLevel
    {
        public int FundingTypeID { get; set; }
        public int FundingLevelID { get; set; }
        public bool IsActive { get; set; }
    }
}
