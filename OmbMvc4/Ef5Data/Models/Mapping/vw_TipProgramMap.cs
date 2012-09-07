using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class vw_TipProgramMap : EntityTypeConfiguration<vw_TipProgram>
    {
        public vw_TipProgramMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TIPProgramID, t.TimePeriodID, t.TimePeriod });

            // Properties
            this.Property(t => t.TIPProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Notes)
                .HasMaxLength(255);

            this.Property(t => t.TimePeriod)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("vw_TipProgram");
            this.Property(t => t.TIPProgramID).HasColumnName("TIPProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.PublicHearingDate).HasColumnName("PublicHearingDate");
            this.Property(t => t.AdoptionDate).HasColumnName("AdoptionDate");
            this.Property(t => t.LastAmendmentDate).HasColumnName("LastAmendmentDate");
            this.Property(t => t.GovernorApprovalDate).HasColumnName("GovernorApprovalDate");
            this.Property(t => t.USDOTApprovalDate).HasColumnName("USDOTApprovalDate");
            this.Property(t => t.USEPAApprovalDate).HasColumnName("USEPAApprovalDate");
            this.Property(t => t.Current).HasColumnName("Current");
            this.Property(t => t.Pending).HasColumnName("Pending");
            this.Property(t => t.Previous).HasColumnName("Previous");
            this.Property(t => t.OpeningDate).HasColumnName("OpeningDate");
            this.Property(t => t.ClosingDate).HasColumnName("ClosingDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.TimePeriod).HasColumnName("TimePeriod");
        }
    }
}
