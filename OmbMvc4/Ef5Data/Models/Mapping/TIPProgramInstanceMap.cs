using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProgramInstanceMap : EntityTypeConfiguration<TIPProgramInstance>
    {
        public TIPProgramInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TIPProgramID, t.TimePeriodID });

            // Properties
            this.Property(t => t.TIPProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TIPProgramInstance");
            this.Property(t => t.TIPProgramID).HasColumnName("TIPProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.PublicHearingDate).HasColumnName("PublicHearingDate");
            this.Property(t => t.AdoptionDate).HasColumnName("AdoptionDate");
            this.Property(t => t.LastAmendmentDate).HasColumnName("LastAmendmentDate");
            this.Property(t => t.GovernorApprovalDate).HasColumnName("GovernorApprovalDate");
            this.Property(t => t.USDOTApprovalDate).HasColumnName("USDOTApprovalDate");
            this.Property(t => t.USEPAApprovalDate).HasColumnName("USEPAApprovalDate");
            this.Property(t => t.ShowDelayDate).HasColumnName("ShowDelayDate");

            // Relationships
            this.HasRequired(t => t.ProgramInstance)
                .WithOptional(t => t.TIPProgramInstance);

        }
    }
}
