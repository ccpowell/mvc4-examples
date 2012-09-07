using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class GeographyMap : EntityTypeConfiguration<Geography>
    {
        public GeographyMap()
        {
            // Primary Key
            this.HasKey(t => t.GeographyID);

            // Properties
            this.Property(t => t.Geography1)
                .IsRequired()
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("Geography");
            this.Property(t => t.GeographyID).HasColumnName("GeographyID");
            this.Property(t => t.Geography1).HasColumnName("Geography");
            this.Property(t => t.GeographyTypeID).HasColumnName("GeographyTypeID");

            // Relationships
            this.HasRequired(t => t.GeographyType)
                .WithMany(t => t.Geographies)
                .HasForeignKey(d => d.GeographyTypeID);

        }
    }
}
