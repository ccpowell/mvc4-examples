using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectFinancialRecordDetailPhaseMap : EntityTypeConfiguration<ProjectFinancialRecordDetailPhase>
    {
        public ProjectFinancialRecordDetailPhaseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectFinancialRecordID, t.FundingIncrementID, t.FundingResourceID, t.PhaseID });

            // Properties
            this.Property(t => t.ProjectFinancialRecordID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundingIncrementID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundingResourceID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PhaseID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectFinancialRecordDetailPhase");
            this.Property(t => t.ProjectFinancialRecordID).HasColumnName("ProjectFinancialRecordID");
            this.Property(t => t.FundingIncrementID).HasColumnName("FundingIncrementID");
            this.Property(t => t.FundingResourceID).HasColumnName("FundingResourceID");
            this.Property(t => t.PhaseID).HasColumnName("PhaseID");
            this.Property(t => t.IsInitiated).HasColumnName("IsInitiated");
            this.Property(t => t.IsChecked).HasColumnName("IsChecked");
            this.Property(t => t.MidYearStatus).HasColumnName("MidYearStatus");
            this.Property(t => t.EndYearStatus).HasColumnName("EndYearStatus");
            this.Property(t => t.ActionPlan).HasColumnName("ActionPlan");
            this.Property(t => t.MeetingDate).HasColumnName("MeetingDate");
            this.Property(t => t.Notes).HasColumnName("Notes");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithMany(t => t.ProjectFinancialRecordDetailPhases)
                .HasForeignKey(d => d.PhaseID);
            this.HasRequired(t => t.FundingIncrement)
                .WithMany(t => t.ProjectFinancialRecordDetailPhases)
                .HasForeignKey(d => d.FundingIncrementID);
            this.HasRequired(t => t.FundingResource)
                .WithMany(t => t.ProjectFinancialRecordDetailPhases)
                .HasForeignKey(d => d.FundingResourceID);

        }
    }
}
