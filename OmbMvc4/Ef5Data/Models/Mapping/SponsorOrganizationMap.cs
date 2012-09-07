using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class SponsorOrganizationMap : EntityTypeConfiguration<SponsorOrganization>
    {
        public SponsorOrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.SponsorOrganizationID);

            // Properties
            this.Property(t => t.SponsorOrganizationID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TransportationCodeID)
                .HasMaxLength(50);

            this.Property(t => t.ProjectPrefix)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SponsorOrganization");
            this.Property(t => t.SponsorOrganizationID).HasColumnName("SponsorOrganizationID");
            this.Property(t => t.AdministrativeTypeID).HasColumnName("AdministrativeTypeID");
            this.Property(t => t.TransportationCodeID).HasColumnName("TransportationCodeID");
            this.Property(t => t.ProjectPrefix).HasColumnName("ProjectPrefix");
            this.Property(t => t.DRCOGContactID).HasColumnName("DRCOGContactID");
            this.Property(t => t.Temp_PrintCertificationFormDate).HasColumnName("Temp_PrintCertificationFormDate");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.SponsorOrganizations)
                .HasForeignKey(d => d.AdministrativeTypeID);
            this.HasRequired(t => t.Organization)
                .WithOptional(t => t.SponsorOrganization);
            this.HasOptional(t => t.Person)
                .WithMany(t => t.SponsorOrganizations)
                .HasForeignKey(d => d.DRCOGContactID);

        }
    }
}
