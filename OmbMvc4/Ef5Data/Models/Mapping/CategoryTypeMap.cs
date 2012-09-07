using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CategoryTypeMap : EntityTypeConfiguration<CategoryType>
    {
        public CategoryTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryTypeID);

            // Properties
            this.Property(t => t.CategoryType1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CategoryType");
            this.Property(t => t.CategoryTypeID).HasColumnName("CategoryTypeID");
            this.Property(t => t.CategoryType1).HasColumnName("CategoryType");
            this.Property(t => t.ParentCategoryTypeID).HasColumnName("ParentCategoryTypeID");
        }
    }
}
