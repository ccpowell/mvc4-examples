using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectMuniGeographyMap : EntityTypeConfiguration<ProjectMuniGeography>
    {
        public ProjectMuniGeographyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MuniGeographyID, t.ProjectID });

            // Properties
            this.Property(t => t.MuniGeographyID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectMuniGeography");
            this.Property(t => t.MuniGeographyID).HasColumnName("MuniGeographyID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Share).HasColumnName("Share");
            this.Property(t => t.Primary).HasColumnName("Primary");

            // Relationships
            this.HasRequired(t => t.MuniGeography)
                .WithMany(t => t.ProjectMuniGeographies)
                .HasForeignKey(d => d.MuniGeographyID);
            this.HasRequired(t => t.Project)
                .WithMany(t => t.ProjectMuniGeographies)
                .HasForeignKey(d => d.ProjectID);

        }
    }
}
