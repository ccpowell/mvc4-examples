using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class RTPProgramInstanceSponsor
    {
        public int RTPProgramID { get; set; }
        public int SponsorID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<System.DateTime> ReadyDate { get; set; }
        public Nullable<System.DateTime> EmailDate { get; set; }
        public Nullable<System.DateTime> FormPrintedDate { get; set; }
        public virtual ProgramInstanceSponsor ProgramInstanceSponsor { get; set; }
        public virtual RTPProgramInstance RTPProgramInstance { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
    }
}
