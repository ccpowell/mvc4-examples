using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.Models.Survey
{
    public class SearchModel : ProjectSearchModel
    {
        public Profile Profile { get; set; }

        public string Year { get; set; }
        public int YearId { get; set; }
        //public string RtpID { get; set; }
        //public string RtpYear { get; set; }
        //public int? RtpYearID { get; set; }
        //public int NetworkID { get; set; }

        //public int PlanTypeId { get; set; }
        //public string PlanType { get; set; }

        //public bool Exclude_PlanType { get; set; }

        //public string TipId { get; set; }
        //public bool Exclude_TipId { get; set; }

        //public bool RequireTipId { get; set; }

        public bool ShowMySponsorAgencies { get; set; }
        public bool ShowAllForAgency { get; set; }
        public int PersonId { get; set; }
        
    }
}
