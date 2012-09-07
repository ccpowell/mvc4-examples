using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProgramInstanceSponsorMap : EntityTypeConfiguration<TIPProgramInstanceSponsor>
    {
        public TIPProgramInstanceSponsorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TIPProgramID, t.SponsorID, t.TimePeriodID });

            // Properties
            this.Property(t => t.TIPProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SponsorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TIPProgramInstanceSponsor");
            this.Property(t => t.TIPProgramID).HasColumnName("TIPProgramID");
            this.Property(t => t.SponsorID).HasColumnName("SponsorID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.AlternativeSponsorCapable).HasColumnName("AlternativeSponsorCapable");
            this.Property(t => t.PrintCertificationFormDate).HasColumnName("PrintCertificationFormDate");
            this.Property(t => t.SponsorImmunityProjectID).HasColumnName("SponsorImmunityProjectID");
            this.Property(t => t.SponsorImmunityDate).HasColumnName("SponsorImmunityDate");

            // Relationships
            this.HasRequired(t => t.ProgramInstanceSponsor)
                .WithOptional(t => t.TIPProgramInstanceSponsor);
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.TIPProgramInstanceSponsors)
                .HasForeignKey(d => d.SponsorID);

        }
    }
}
