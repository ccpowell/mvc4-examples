using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TIPProgramInstance
    {
        public int TIPProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<System.DateTime> PublicHearingDate { get; set; }
        public Nullable<System.DateTime> AdoptionDate { get; set; }
        public Nullable<System.DateTime> LastAmendmentDate { get; set; }
        public Nullable<System.DateTime> GovernorApprovalDate { get; set; }
        public Nullable<System.DateTime> USDOTApprovalDate { get; set; }
        public Nullable<System.DateTime> USEPAApprovalDate { get; set; }
        public Nullable<System.DateTime> ShowDelayDate { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
    }
}
