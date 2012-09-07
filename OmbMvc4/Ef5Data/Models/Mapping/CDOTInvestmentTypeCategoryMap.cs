using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CDOTInvestmentTypeCategoryMap : EntityTypeConfiguration<CDOTInvestmentTypeCategory>
    {
        public CDOTInvestmentTypeCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CDOTInvestmentTypeCategoryID);

            // Properties
            this.Property(t => t.CDOTInvestmentTypeCategoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .HasMaxLength(50);

            this.Property(t => t.Group)
                .HasMaxLength(50);

            this.Property(t => t.ImprovementType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CDOTInvestmentTypeCategory");
            this.Property(t => t.CDOTInvestmentTypeCategoryID).HasColumnName("CDOTInvestmentTypeCategoryID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.ImprovementType).HasColumnName("ImprovementType");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithOptional(t => t.CDOTInvestmentTypeCategory);

        }
    }
}
