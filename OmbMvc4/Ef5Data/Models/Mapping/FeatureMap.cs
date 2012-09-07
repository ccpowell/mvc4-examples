using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class FeatureMap : EntityTypeConfiguration<Feature>
    {
        public FeatureMap()
        {
            // Primary Key
            this.HasKey(t => t.FeatureID);

            // Properties
            this.Property(t => t.Feature1)
                .HasMaxLength(255);

            this.Property(t => t.Selectability)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Feature");
            this.Property(t => t.FeatureID).HasColumnName("FeatureID");
            this.Property(t => t.Feature1).HasColumnName("Feature");
            this.Property(t => t.FeatureTypeID).HasColumnName("FeatureTypeID");
            this.Property(t => t.PedPoints).HasColumnName("PedPoints");
            this.Property(t => t.BikePoints).HasColumnName("BikePoints");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
            this.Property(t => t.Points).HasColumnName("Points");
            this.Property(t => t.Selectability).HasColumnName("Selectability");

            // Relationships
            this.HasMany(t => t.TIPProjectEvaluation2010)
                .WithMany(t => t.Features)
                .Map(m =>
                    {
                        m.ToTable("TIPProjectEvaluation2010Feature");
                        m.MapLeftKey("FeatureID");
                        m.MapRightKey("ProjectVersionID");
                    });

            this.HasOptional(t => t.FeatureType)
                .WithMany(t => t.Features)
                .HasForeignKey(d => d.FeatureTypeID);

        }
    }
}
