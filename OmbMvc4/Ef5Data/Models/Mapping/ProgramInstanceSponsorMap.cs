using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProgramInstanceSponsorMap : EntityTypeConfiguration<ProgramInstanceSponsor>
    {
        public ProgramInstanceSponsorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProgramID, t.TimePeriodID, t.SponsorID });

            // Properties
            this.Property(t => t.ProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SponsorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProgramInstanceSponsor");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.SponsorID).HasColumnName("SponsorID");

            // Relationships
            this.HasRequired(t => t.ProgramInstance)
                .WithMany(t => t.ProgramInstanceSponsors)
                .HasForeignKey(d => new { d.ProgramID, d.TimePeriodID });
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.ProgramInstanceSponsors)
                .HasForeignKey(d => d.SponsorID);

        }
    }
}
