using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.OrganizationID);

            // Properties
            this.Property(t => t.OrganizationName)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.LegalName)
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("Organization");
            this.Property(t => t.OrganizationID).HasColumnName("OrganizationID");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.Temp_OldAgencyID).HasColumnName("Temp_OldAgencyID");
            this.Property(t => t.LegalName).HasColumnName("LegalName");

            // Relationships
            this.HasMany(t => t.OrganizationTypes)
                .WithMany(t => t.Organizations)
                .Map(m =>
                    {
                        m.ToTable("OrganizationOrganizationType");
                        m.MapLeftKey("OrganizationID");
                        m.MapRightKey("OrganizationTypeID");
                    });


        }
    }
}
