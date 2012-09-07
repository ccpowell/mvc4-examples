using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ProfilePropertyValueMap : EntityTypeConfiguration<ProfilePropertyValue>
    {
        public ProfilePropertyValueMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfilePropertyValueID);

            // Properties
            this.Property(t => t.ProfilePropertyValue1)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ProfilePropertyValue");
            this.Property(t => t.ProfilePropertyValueID).HasColumnName("ProfilePropertyValueID");
            this.Property(t => t.ProfilePropertyID).HasColumnName("ProfilePropertyID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.ProfilePropertyValue1).HasColumnName("ProfilePropertyValue");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.DateUpdated).HasColumnName("DateUpdated");

            // Relationships
            this.HasRequired(t => t.aspnet_Users)
                .WithMany(t => t.ProfilePropertyValues)
                .HasForeignKey(d => d.UserID);
            this.HasRequired(t => t.ProfileProperty)
                .WithMany(t => t.ProfilePropertyValues)
                .HasForeignKey(d => d.ProfilePropertyID);

        }
    }
}
