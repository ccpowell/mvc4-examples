using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RTPProgramInstanceSponsorMap : EntityTypeConfiguration<RTPProgramInstanceSponsor>
    {
        public RTPProgramInstanceSponsorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RTPProgramID, t.SponsorID, t.TimePeriodID });

            // Properties
            this.Property(t => t.RTPProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SponsorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("RTPProgramInstanceSponsor");
            this.Property(t => t.RTPProgramID).HasColumnName("RTPProgramID");
            this.Property(t => t.SponsorID).HasColumnName("SponsorID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.ReadyDate).HasColumnName("ReadyDate");
            this.Property(t => t.EmailDate).HasColumnName("EmailDate");
            this.Property(t => t.FormPrintedDate).HasColumnName("FormPrintedDate");

            // Relationships
            this.HasRequired(t => t.ProgramInstanceSponsor)
                .WithOptional(t => t.RTPProgramInstanceSponsor);
            this.HasRequired(t => t.RTPProgramInstance)
                .WithMany(t => t.RTPProgramInstanceSponsors)
                .HasForeignKey(d => new { d.RTPProgramID, d.TimePeriodID });
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.RTPProgramInstanceSponsors)
                .HasForeignKey(d => d.SponsorID);

        }
    }
}
