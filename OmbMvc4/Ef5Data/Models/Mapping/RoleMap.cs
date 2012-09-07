using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleID);

            // Properties
            this.Property(t => t.Role1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.Role1).HasColumnName("Role");
            this.Property(t => t.RoleTypeID).HasColumnName("RoleTypeID");

            // Relationships
            this.HasRequired(t => t.RoleType)
                .WithMany(t => t.Roles)
                .HasForeignKey(d => d.RoleTypeID);

        }
    }
}
