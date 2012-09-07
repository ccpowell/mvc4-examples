using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class OrganizationPersonRoleMap : EntityTypeConfiguration<OrganizationPersonRole>
    {
        public OrganizationPersonRoleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrganizationID, t.PersonID, t.RoleID });

            // Properties
            this.Property(t => t.OrganizationID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PersonID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("OrganizationPersonRole");
            this.Property(t => t.OrganizationID).HasColumnName("OrganizationID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.EndDate).HasColumnName("EndDate");

            // Relationships
            this.HasRequired(t => t.Organization)
                .WithMany(t => t.OrganizationPersonRoles)
                .HasForeignKey(d => d.OrganizationID);
            this.HasRequired(t => t.Person)
                .WithMany(t => t.OrganizationPersonRoles)
                .HasForeignKey(d => d.PersonID);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.OrganizationPersonRoles)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
