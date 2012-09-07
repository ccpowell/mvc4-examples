using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LRSCategoryMap : EntityTypeConfiguration<LRSCategory>
    {
        public LRSCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LRSCategory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryTypeId).HasColumnName("CategoryTypeId");
            this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            this.HasRequired(t => t.LRSCategoryType)
                .WithMany(t => t.LRSCategories)
                .HasForeignKey(d => d.CategoryTypeId);

        }
    }
}
