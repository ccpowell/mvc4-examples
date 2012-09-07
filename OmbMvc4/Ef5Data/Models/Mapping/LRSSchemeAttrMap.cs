using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LRSSchemeAttrMap : EntityTypeConfiguration<LRSSchemeAttr>
    {
        public LRSSchemeAttrMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.COLUMN_NAME)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.FRIENDLY_NAME)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LRSSchemeAttr");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LRSSchemeId).HasColumnName("LRSSchemeId");
            this.Property(t => t.COLUMN_NAME).HasColumnName("COLUMN_NAME");
            this.Property(t => t.FRIENDLY_NAME).HasColumnName("FRIENDLY_NAME");
            this.Property(t => t.DATA_TYPE).HasColumnName("DATA_TYPE");
            this.Property(t => t.DISPLAY_TYPE).HasColumnName("DISPLAY_TYPE");
            this.Property(t => t.CHARACTER_MAXIMUM_LENGTH).HasColumnName("CHARACTER_MAXIMUM_LENGTH");
            this.Property(t => t.COLUMN_DEFAULT).HasColumnName("COLUMN_DEFAULT");
            this.Property(t => t.IS_NULLABLE).HasColumnName("IS_NULLABLE");
            this.Property(t => t.IS_REQUIRED).HasColumnName("IS_REQUIRED");

            // Relationships
            this.HasOptional(t => t.LRSCategory)
                .WithMany(t => t.LRSSchemeAttrs)
                .HasForeignKey(d => d.DATA_TYPE);
            this.HasOptional(t => t.LRSCategory1)
                .WithMany(t => t.LRSSchemeAttrs1)
                .HasForeignKey(d => d.DISPLAY_TYPE);
            this.HasRequired(t => t.LRSScheme)
                .WithMany(t => t.LRSSchemeAttrs)
                .HasForeignKey(d => d.LRSSchemeId);

        }
    }
}
