using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProfileAttributeMap : EntityTypeConfiguration<ProfileAttribute>
    {
        public ProfileAttributeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AttributeId, t.ProfileId });

            // Properties
            this.Property(t => t.ProfileId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AttributeValue)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ProfileAttribute");
            this.Property(t => t.AttributeId).HasColumnName("AttributeId");
            this.Property(t => t.ProfileId).HasColumnName("ProfileId");
            this.Property(t => t.AttributeValue).HasColumnName("AttributeValue");

            // Relationships
            this.HasRequired(t => t.Profile)
                .WithMany(t => t.ProfileAttributes)
                .HasForeignKey(d => d.ProfileId);
            this.HasRequired(t => t.ProfileAttributeType)
                .WithMany(t => t.ProfileAttributes)
                .HasForeignKey(d => d.AttributeId);

        }
    }
}
