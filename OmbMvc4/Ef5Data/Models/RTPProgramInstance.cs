using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RTPProgramInstance
    {
        public RTPProgramInstance()
        {
            this.RTPProgramInstanceSponsors = new List<RTPProgramInstanceSponsor>();
        }

        public int RTPProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<System.DateTime> AdoptionDate { get; set; }
        public Nullable<System.DateTime> LastAmendmentDate { get; set; }
        public Nullable<System.DateTime> PublicHearingDate { get; set; }
        public Nullable<System.DateTime> CDOTActionDate { get; set; }
        public Nullable<System.DateTime> USDOTApprovalDate { get; set; }
        public string Description { get; set; }
        public Nullable<short> BaseYearID { get; set; }
        public Nullable<short> PlanStartYearID { get; set; }
        public Nullable<short> PlanEndYearID { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
        public virtual TimePeriod TimePeriod1 { get; set; }
        public virtual TimePeriod TimePeriod2 { get; set; }
        public virtual ICollection<RTPProgramInstanceSponsor> RTPProgramInstanceSponsors { get; set; }
    }
}
