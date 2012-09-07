using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class MetroVisionMeasureSponsorMap : EntityTypeConfiguration<MetroVisionMeasureSponsor>
    {
        public MetroVisionMeasureSponsorMap()
        {
            // Primary Key
            this.HasKey(t => t.SpecificMetroVisionMeasureID);

            // Properties
            // Table & Column Mappings
            this.ToTable("MetroVisionMeasureSponsor");
            this.Property(t => t.SpecificMetroVisionMeasureID).HasColumnName("SpecificMetroVisionMeasureID");
            this.Property(t => t.MetroVisionMeasureID).HasColumnName("MetroVisionMeasureID");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.SponsorId).HasColumnName("SponsorId");

            // Relationships
            this.HasRequired(t => t.MetroVisionMeasure)
                .WithMany(t => t.MetroVisionMeasureSponsors)
                .HasForeignKey(d => d.MetroVisionMeasureID);
            this.HasOptional(t => t.ProjectVersion)
                .WithMany(t => t.MetroVisionMeasureSponsors)
                .HasForeignKey(d => d.ProjectVersionID);
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.MetroVisionMeasureSponsors)
                .HasForeignKey(d => d.SponsorId);

        }
    }
}
