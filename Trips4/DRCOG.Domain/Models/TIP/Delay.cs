using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DRCOG.Domain.Models.TIP
{
    public class Delay
    {
        public int ProjectVersionId { get; set; }
        public int TimePeriodId { get; set; }
        public string TimePeriod { get; set; }

        public string Year { get; set; }
        public string TipId { get; set; }
        public string Sponsor { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        public string Phase { get; set; }
        [DisplayName("Federal Amount")]
        public double FederalAmount { get; set; }

        [DisplayName("Additional Notes")]
        public string Notes { get; set; }
        [DisplayName("Mid-Year Status")]
        public string MidYearStatus { get; set; }
        [DisplayName("End of Year Status")]
        public string EndYearStatus { get; set; }
        [DisplayName("Action Plan")]
        public string ActionPlan { get; set; }
        [DisplayName("Meeting Date")]
        public DateTime? MeetingDate { get; set; }
        [DisplayName("Has this phase been initiated?")]
        public bool IsInitiated { get; set; }
        [DisplayName("Mark as Checked?")]
        public bool IsChecked { get; set; }
        public bool IsDelay { get; set; }

        public string AffectedProjectDelaysLocation { get; set; }

        public int ProjectFinancialRecordId { get; set; }
        public int FundingIncrementId { get; set; }
        public int FundingResourceId { get; set; }
        public int PhaseId { get; set; }
    }
}


