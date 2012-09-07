using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProgramInstanceSponsor
    {
        public int ProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public int SponsorID { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual SponsorOrganization SponsorOrganization { get; set; }
        public virtual RTPProgramInstanceSponsor RTPProgramInstanceSponsor { get; set; }
        public virtual SurveyProgramInstanceSponsor SurveyProgramInstanceSponsor { get; set; }
        public virtual TIPProgramInstanceSponsor TIPProgramInstanceSponsor { get; set; }
    }
}
