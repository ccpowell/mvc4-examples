using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectFinancialRecordMap : EntityTypeConfiguration<ProjectFinancialRecord>
    {
        public ProjectFinancialRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectFinancialRecordID);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProjectFinancialRecord");
            this.Property(t => t.ProjectFinancialRecordID).HasColumnName("ProjectFinancialRecordID");
            this.Property(t => t.FundPeriodID).HasColumnName("FundPeriodID");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.Previous).HasColumnName("Previous");
            this.Property(t => t.Future).HasColumnName("Future");
            this.Property(t => t.TIPFunding).HasColumnName("TIPFunding");
            this.Property(t => t.FederalTotal).HasColumnName("FederalTotal");
            this.Property(t => t.StateTotal).HasColumnName("StateTotal");
            this.Property(t => t.LocalTotal).HasColumnName("LocalTotal");
            this.Property(t => t.TotalCost).HasColumnName("TotalCost");
            this.Property(t => t.Temp_PreviousAmendID).HasColumnName("Temp_PreviousAmendID");

            // Relationships
            this.HasRequired(t => t.ProjectVersion)
                .WithMany(t => t.ProjectFinancialRecords)
                .HasForeignKey(d => d.ProjectVersionID);

        }
    }
}
