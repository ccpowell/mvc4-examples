using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RTPReportGroupingCategoryMap : EntityTypeConfiguration<RTPReportGroupingCategory>
    {
        public RTPReportGroupingCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.RTPReportGroupingCategoryID);

            // Properties
            this.Property(t => t.RTPReportGroupingCategoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShortTitle)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RTPReportGroupingCategory");
            this.Property(t => t.RTPReportGroupingCategoryID).HasColumnName("RTPReportGroupingCategoryID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.ShortTitle).HasColumnName("ShortTitle");
            this.Property(t => t.Subtotals).HasColumnName("Subtotals");
            this.Property(t => t.ListDisplay).HasColumnName("ListDisplay");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithOptional(t => t.RTPReportGroupingCategory);
            this.HasOptional(t => t.TimePeriod)
                .WithMany(t => t.RTPReportGroupingCategories)
                .HasForeignKey(d => d.TimePeriodID);

        }
    }
}
