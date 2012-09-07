using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectCountyGeographyMap : EntityTypeConfiguration<ProjectCountyGeography>
    {
        public ProjectCountyGeographyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CountyGeographyID, t.ProjectID });

            // Properties
            this.Property(t => t.CountyGeographyID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectCountyGeography");
            this.Property(t => t.CountyGeographyID).HasColumnName("CountyGeographyID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Share).HasColumnName("Share");
            this.Property(t => t.Primary).HasColumnName("Primary");

            // Relationships
            this.HasRequired(t => t.CountyGeography)
                .WithMany(t => t.ProjectCountyGeographies)
                .HasForeignKey(d => d.CountyGeographyID);

        }
    }
}
