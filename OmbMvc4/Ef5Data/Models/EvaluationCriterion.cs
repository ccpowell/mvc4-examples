using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class EvaluationCriterion
    {
        public int EvaluationCriterionID { get; set; }
        public string EvaluationCriterion1 { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }
        public Nullable<int> OrderNumber { get; set; }
    }
}
