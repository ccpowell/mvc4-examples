using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.RTP
{
    public class RTPSearchModel : ProjectSearchModel
    {
        public string RtpID { get; set; }
        public string RtpYear { get; set; }
        public int? RtpYearID { get; set; }
        public int NetworkID { get; set; }

        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }

        public bool Exclude_PlanType { get; set; }

        public string TipId { get; set; }
        public bool Exclude_TipId { get; set; }

        public bool ShowCancelledProjects { get; set; }

        public bool RequireTipId { get; set; }

        public int CycleId { get; set; }
        
    }
}
