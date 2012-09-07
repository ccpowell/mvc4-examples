using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectFinancialRecordDetailMap : EntityTypeConfiguration<ProjectFinancialRecordDetail>
    {
        public ProjectFinancialRecordDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectFinancialRecordID, t.FundingIncrementID, t.FundingResourceID });

            // Properties
            this.Property(t => t.ProjectFinancialRecordID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundingIncrementID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundingResourceID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectFinancialRecordDetail");
            this.Property(t => t.ProjectFinancialRecordID).HasColumnName("ProjectFinancialRecordID");
            this.Property(t => t.FundingIncrementID).HasColumnName("FundingIncrementID");
            this.Property(t => t.FundingResourceID).HasColumnName("FundingResourceID");
            this.Property(t => t.FederalAmount).HasColumnName("FederalAmount");
            this.Property(t => t.StateAmount).HasColumnName("StateAmount");
            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount");

            // Relationships
            this.HasRequired(t => t.FundingIncrement)
                .WithMany(t => t.ProjectFinancialRecordDetails)
                .HasForeignKey(d => d.FundingIncrementID);
            this.HasRequired(t => t.ProjectFinancialRecord)
                .WithMany(t => t.ProjectFinancialRecordDetails)
                .HasForeignKey(d => d.ProjectFinancialRecordID);

        }
    }
}
