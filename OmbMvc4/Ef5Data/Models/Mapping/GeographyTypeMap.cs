using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class GeographyTypeMap : EntityTypeConfiguration<GeographyType>
    {
        public GeographyTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.GeographyTypeID);

            // Properties
            this.Property(t => t.GeographyType1)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("GeographyType");
            this.Property(t => t.GeographyTypeID).HasColumnName("GeographyTypeID");
            this.Property(t => t.GeographyType1).HasColumnName("GeographyType");
        }
    }
}
