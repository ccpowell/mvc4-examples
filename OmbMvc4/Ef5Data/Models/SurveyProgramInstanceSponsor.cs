using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class SurveyProgramInstanceSponsor
    {
        public int SurveyProgramID { get; set; }
        public int SponsorID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<System.DateTime> ReadyDate { get; set; }
        public Nullable<System.DateTime> PrintCertificationFormDate { get; set; }
        public Nullable<System.DateTime> SentEmailDate { get; set; }
        public virtual ProgramInstanceSponsor ProgramInstanceSponsor { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
        public virtual SurveyProgramInstance SurveyProgramInstance { get; set; }
    }
}
