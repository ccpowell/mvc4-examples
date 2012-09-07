using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProfileAttributeTypeClassMap : EntityTypeConfiguration<ProfileAttributeTypeClass>
    {
        public ProfileAttributeTypeClassMap()
        {
            // Primary Key
            this.HasKey(t => t.ClassId);

            // Properties
            this.Property(t => t.ClassName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProfileAttributeTypeClass");
            this.Property(t => t.ClassId).HasColumnName("ClassId");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
        }
    }
}
