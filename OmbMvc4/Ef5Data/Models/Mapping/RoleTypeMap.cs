using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RoleTypeMap : EntityTypeConfiguration<RoleType>
    {
        public RoleTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleTypeID);

            // Properties
            this.Property(t => t.RoleType1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RoleType");
            this.Property(t => t.RoleTypeID).HasColumnName("RoleTypeID");
            this.Property(t => t.RoleType1).HasColumnName("RoleType");
        }
    }
}
