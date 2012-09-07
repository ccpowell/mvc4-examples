using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LRSCategoryTypeMap : EntityTypeConfiguration<LRSCategoryType>
    {
        public LRSCategoryTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LRSCategoryType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Category).HasColumnName("Category");
        }
    }
}
