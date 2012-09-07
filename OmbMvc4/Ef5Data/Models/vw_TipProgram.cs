using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class vw_TipProgram
    {
        public int TIPProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<System.DateTime> PublicHearingDate { get; set; }
        public Nullable<System.DateTime> AdoptionDate { get; set; }
        public Nullable<System.DateTime> LastAmendmentDate { get; set; }
        public Nullable<System.DateTime> GovernorApprovalDate { get; set; }
        public Nullable<System.DateTime> USDOTApprovalDate { get; set; }
        public Nullable<System.DateTime> USEPAApprovalDate { get; set; }
        public Nullable<bool> Current { get; set; }
        public Nullable<bool> Pending { get; set; }
        public Nullable<bool> Previous { get; set; }
        public Nullable<System.DateTime> OpeningDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public string Notes { get; set; }
        public string TimePeriod { get; set; }
    }
}
