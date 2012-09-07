using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class MediumMap : EntityTypeConfiguration<Medium>
    {
        public MediumMap()
        {
            // Primary Key
            this.HasKey(t => t.mediaId);

            // Properties
            this.Property(t => t.fileName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.mediaType)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Media");
            this.Property(t => t.mediaId).HasColumnName("mediaId");
            this.Property(t => t.dateCreated).HasColumnName("dateCreated");
            this.Property(t => t.fileName).HasColumnName("fileName");
            this.Property(t => t.mediaType).HasColumnName("mediaType");
            this.Property(t => t.file).HasColumnName("file");
        }
    }
}
