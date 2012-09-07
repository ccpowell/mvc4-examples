using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class EvaluationCriterionProjectType
    {
        public int EvaluationCriterionID { get; set; }
        public string ProjectType { get; set; }
        public Nullable<short> OrderNumber { get; set; }
    }
}
