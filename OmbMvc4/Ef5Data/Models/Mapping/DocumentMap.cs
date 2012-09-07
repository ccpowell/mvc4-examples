using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class DocumentMap : EntityTypeConfiguration<Document>
    {
        public DocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.DocumentID);

            // Properties
            this.Property(t => t.Filename)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Document");
            this.Property(t => t.DocumentID).HasColumnName("DocumentID");
            this.Property(t => t.Filename).HasColumnName("Filename");
            this.Property(t => t.DocumentTypeID).HasColumnName("DocumentTypeID");
            this.Property(t => t.MediaGuid).HasColumnName("MediaGuid");

            // Relationships
            this.HasOptional(t => t.DocumentType)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.DocumentTypeID);

        }
    }
}
