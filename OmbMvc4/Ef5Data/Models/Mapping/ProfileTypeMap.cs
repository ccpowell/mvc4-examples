using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProfileTypeMap : EntityTypeConfiguration<ProfileType>
    {
        public ProfileTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfileTypeID);

            // Properties
            this.Property(t => t.ProfileType1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProfileType");
            this.Property(t => t.ProfileTypeID).HasColumnName("ProfileTypeID");
            this.Property(t => t.ProfileType1).HasColumnName("ProfileType");
        }
    }
}
