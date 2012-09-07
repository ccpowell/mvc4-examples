using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TIPProgramInstanceSponsor
    {
        public int TIPProgramID { get; set; }
        public int SponsorID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<bool> AlternativeSponsorCapable { get; set; }
        public Nullable<System.DateTime> PrintCertificationFormDate { get; set; }
        public Nullable<int> SponsorImmunityProjectID { get; set; }
        public Nullable<System.DateTime> SponsorImmunityDate { get; set; }
        public virtual ProgramInstanceSponsor ProgramInstanceSponsor { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
    }
}
