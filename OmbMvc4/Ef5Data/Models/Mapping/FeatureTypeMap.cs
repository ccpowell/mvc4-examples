using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FeatureTypeMap : EntityTypeConfiguration<FeatureType>
    {
        public FeatureTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.FeatureTypeID);

            // Properties
            this.Property(t => t.FeatureType1)
                .HasMaxLength(100);

            this.Property(t => t.ProjectType)
                .HasMaxLength(50);

            this.Property(t => t.Selectability)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FeatureType");
            this.Property(t => t.FeatureTypeID).HasColumnName("FeatureTypeID");
            this.Property(t => t.FeatureType1).HasColumnName("FeatureType");
            this.Property(t => t.ProjectType).HasColumnName("ProjectType");
            this.Property(t => t.Selectability).HasColumnName("Selectability");
        }
    }
}
