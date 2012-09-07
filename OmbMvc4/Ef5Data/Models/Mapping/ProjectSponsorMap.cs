using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectSponsorMap : EntityTypeConfiguration<ProjectSponsor>
    {
        public ProjectSponsorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectID, t.SponsorID });

            // Properties
            this.Property(t => t.ProjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SponsorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectSponsor");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.SponsorID).HasColumnName("SponsorID");
            this.Property(t => t.Primary).HasColumnName("Primary");

            // Relationships
            this.HasRequired(t => t.Project)
                .WithMany(t => t.ProjectSponsors)
                .HasForeignKey(d => d.ProjectID);
            this.HasRequired(t => t.SponsorOrganization)
                .WithMany(t => t.ProjectSponsors)
                .HasForeignKey(d => d.SponsorID);

        }
    }
}
