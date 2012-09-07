using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class SurveyProgramInstance
    {
        public SurveyProgramInstance()
        {
            this.SurveyProgramInstanceSponsors = new List<SurveyProgramInstanceSponsor>();
        }

        public int SurveyProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public Nullable<int> PreviousTimePeriodID { get; set; }
        public Nullable<System.DateTime> AcceptedDate { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual ICollection<SurveyProgramInstanceSponsor> SurveyProgramInstanceSponsors { get; set; }
    }
}
