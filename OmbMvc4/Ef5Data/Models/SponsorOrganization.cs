using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class SponsorOrganization
    {
        public SponsorOrganization()
        {
            this.MetroVisionMeasureSponsors = new List<MetroVisionMeasureSponsor>();
            this.ProgramInstanceSponsors = new List<ProgramInstanceSponsor>();
            this.ProjectSponsors = new List<ProjectSponsor>();
            this.RTPProgramInstanceSponsors = new List<RTPProgramInstanceSponsor>();
            this.SurveyProgramInstanceSponsors = new List<SurveyProgramInstanceSponsor>();
            this.TIPProgramInstanceSponsors = new List<TIPProgramInstanceSponsor>();
        }

        public int SponsorOrganizationID { get; set; }
        public Nullable<int> AdministrativeTypeID { get; set; }
        public string TransportationCodeID { get; set; }
        public string ProjectPrefix { get; set; }
        public Nullable<int> DRCOGContactID { get; set; }
        public Nullable<System.DateTime> Temp_PrintCertificationFormDate { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<MetroVisionMeasureSponsor> MetroVisionMeasureSponsors { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<ProgramInstanceSponsor> ProgramInstanceSponsors { get; set; }
        public virtual ICollection<ProjectSponsor> ProjectSponsors { get; set; }
        public virtual ICollection<RTPProgramInstanceSponsor> RTPProgramInstanceSponsors { get; set; }
        public virtual ICollection<SurveyProgramInstanceSponsor> SurveyProgramInstanceSponsors { get; set; }
        public virtual ICollection<TIPProgramInstanceSponsor> TIPProgramInstanceSponsors { get; set; }
    }
}
