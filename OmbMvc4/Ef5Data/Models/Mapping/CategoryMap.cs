using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryID);

            // Properties
            this.Property(t => t.Category1)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.Code)
                .HasMaxLength(5);

            this.Property(t => t.Description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Category");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");
            this.Property(t => t.Category1).HasColumnName("Category");
            this.Property(t => t.CategoryTypeID).HasColumnName("CategoryTypeID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasOptional(t => t.CategoryType)
                .WithMany(t => t.Categories)
                .HasForeignKey(d => d.CategoryTypeID);

        }
    }
}
