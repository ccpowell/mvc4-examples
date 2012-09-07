using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ProfilePropertyMap : EntityTypeConfiguration<ProfileProperty>
    {
        public ProfilePropertyMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfilePropertyID);

            // Properties
            this.Property(t => t.ProfilePropertyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ProfilePropertyType)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ProfileProperty");
            this.Property(t => t.ProfilePropertyID).HasColumnName("ProfilePropertyID");
            this.Property(t => t.ProfilePropertyName).HasColumnName("ProfilePropertyName");
            this.Property(t => t.ProfilePropertyType).HasColumnName("ProfilePropertyType");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.DateUpdated).HasColumnName("DateUpdated");
        }
    }
}
