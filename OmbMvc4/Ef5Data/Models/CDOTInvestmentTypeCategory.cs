using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class CDOTInvestmentTypeCategory
    {
        public int CDOTInvestmentTypeCategoryID { get; set; }
        public string Code { get; set; }
        public string Group { get; set; }
        public string ImprovementType { get; set; }
        public virtual Category Category { get; set; }
    }
}
