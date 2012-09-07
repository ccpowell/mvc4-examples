using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class validation_GetProjectSponsorsMap : EntityTypeConfiguration<validation_GetProjectSponsors>
    {
        public validation_GetProjectSponsorsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectID, t.COGID, t.OrganizationName });

            // Properties
            this.Property(t => t.ProjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.COGID)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.OrganizationName)
                .IsRequired()
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("validation_GetProjectSponsors");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.COGID).HasColumnName("COGID");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.Primary).HasColumnName("Primary");
        }
    }
}
