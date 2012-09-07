using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class SurveyProgramInstanceSponsorMap : EntityTypeConfiguration<SurveyProgramInstanceSponsor>
    {
        public SurveyProgramInstanceSponsorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SurveyProgramID, t.SponsorID, t.TimePeriodID });

            // Properties
            this.Property(t => t.SurveyProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SponsorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SurveyProgramInstanceSponsor");
            this.Property(t => t.SurveyProgramID).HasColumnName("SurveyProgramID");
            this.Property(t => t.SponsorID).HasColumnName("SponsorID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.ReadyDate).HasColumnName("ReadyDate");
            this.Property(t => t.PrintCertificationFormDate).HasColumnName("PrintCertificationFormDate");
            this.Property(t => t.SentEmailDate).HasColumnName("SentEmailDate");

            // Relationships
            this.HasRequired(t => t.ProgramInstanceSponsor)
                .WithOptional(t => t.SurveyProgramInstanceSponsor);
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.SurveyProgramInstanceSponsors)
                .HasForeignKey(d => d.SponsorID);
            this.HasRequired(t => t.SurveyProgramInstance)
                .WithMany(t => t.SurveyProgramInstanceSponsors)
                .HasForeignKey(d => new { d.SurveyProgramID, d.TimePeriodID });

        }
    }
}
