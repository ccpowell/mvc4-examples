using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RTPProgramInstanceMap : EntityTypeConfiguration<RTPProgramInstance>
    {
        public RTPProgramInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RTPProgramID, t.TimePeriodID });

            // Properties
            this.Property(t => t.RTPProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("RTPProgramInstance");
            this.Property(t => t.RTPProgramID).HasColumnName("RTPProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.AdoptionDate).HasColumnName("AdoptionDate");
            this.Property(t => t.LastAmendmentDate).HasColumnName("LastAmendmentDate");
            this.Property(t => t.PublicHearingDate).HasColumnName("PublicHearingDate");
            this.Property(t => t.CDOTActionDate).HasColumnName("CDOTActionDate");
            this.Property(t => t.USDOTApprovalDate).HasColumnName("USDOTApprovalDate");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.BaseYearID).HasColumnName("BaseYearID");
            this.Property(t => t.PlanStartYearID).HasColumnName("PlanStartYearID");
            this.Property(t => t.PlanEndYearID).HasColumnName("PlanEndYearID");

            // Relationships
            this.HasRequired(t => t.ProgramInstance)
                .WithOptional(t => t.RTPProgramInstance);
            this.HasOptional(t => t.TimePeriod)
                .WithMany(t => t.RTPProgramInstances)
                .HasForeignKey(d => d.BaseYearID);
            this.HasOptional(t => t.TimePeriod1)
                .WithMany(t => t.RTPProgramInstances1)
                .HasForeignKey(d => d.PlanStartYearID);
            this.HasOptional(t => t.TimePeriod2)
                .WithMany(t => t.RTPProgramInstances2)
                .HasForeignKey(d => d.PlanEndYearID);

        }
    }
}
