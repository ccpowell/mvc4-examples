using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class MetroVisionMeasureMap : EntityTypeConfiguration<MetroVisionMeasure>
    {
        public MetroVisionMeasureMap()
        {
            // Primary Key
            this.HasKey(t => t.MetroVisionMeasureID);

            // Properties
            this.Property(t => t.MetroVisionMeasure1)
                .HasMaxLength(255);

            this.Property(t => t.TIPYear)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("MetroVisionMeasure");
            this.Property(t => t.MetroVisionMeasureID).HasColumnName("MetroVisionMeasureID");
            this.Property(t => t.MetroVisionMeasure1).HasColumnName("MetroVisionMeasure");
            this.Property(t => t.ProjectLevel).HasColumnName("ProjectLevel");
            this.Property(t => t.TIPYear).HasColumnName("TIPYear");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
