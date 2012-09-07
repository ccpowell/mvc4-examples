using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class GISCategoryTypeMap : EntityTypeConfiguration<GISCategoryType>
    {
        public GISCategoryTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.GISCategoryTypeID);

            // Properties
            this.Property(t => t.GISCategoryType1)
                .IsRequired()
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("GISCategoryType");
            this.Property(t => t.GISCategoryTypeID).HasColumnName("GISCategoryTypeID");
            this.Property(t => t.GISCategoryType1).HasColumnName("GISCategoryType");
        }
    }
}
