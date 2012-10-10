using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.Survey
{
    public class SurveyOverview
    {


        public int ProjectVersionId { get; set; }
        public string COGID { get; set; }
        public string OrganizationName { get; set; }
        public string ProjectName { get; set; }
        public string ImprovementType { get; set; }
        public string Network { get; set; }
        public string OpenYear { get; set; }
        public string FacilityName { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public int LanesBase { get; set; }
        public int LanesFuture { get; set; }
        public string FacilityType { get; set; }
        public bool ModelingCheck { get; set; }
        public string LRSRouteName { get; set; }
        public string LRSBeginMeasure { get; set; }
        public string LRSEndMeasure { get; set; }
    }
}
