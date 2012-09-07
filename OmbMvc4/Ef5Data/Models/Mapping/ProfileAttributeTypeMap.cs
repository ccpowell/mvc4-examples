using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProfileAttributeTypeMap : EntityTypeConfiguration<ProfileAttributeType>
    {
        public ProfileAttributeTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.AttributeId);

            // Properties
            this.Property(t => t.AttributeName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProfileAttributeType");
            this.Property(t => t.AttributeId).HasColumnName("AttributeId");
            this.Property(t => t.AttributeName).HasColumnName("AttributeName");
            this.Property(t => t.ClassId).HasColumnName("ClassId");

            // Relationships
            this.HasRequired(t => t.ProfileAttributeTypeClass)
                .WithMany(t => t.ProfileAttributeTypes)
                .HasForeignKey(d => d.ClassId);

        }
    }
}
