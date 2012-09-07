using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class OrganizationTypeMap : EntityTypeConfiguration<OrganizationType>
    {
        public OrganizationTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.OrganizationTypeID);

            // Properties
            this.Property(t => t.OrganizationType1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OrganizationType");
            this.Property(t => t.OrganizationTypeID).HasColumnName("OrganizationTypeID");
            this.Property(t => t.OrganizationType1).HasColumnName("OrganizationType");
        }
    }
}
