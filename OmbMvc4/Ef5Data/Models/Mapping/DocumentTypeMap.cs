using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class DocumentTypeMap : EntityTypeConfiguration<DocumentType>
    {
        public DocumentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.DocumentTypeID);

            // Properties
            this.Property(t => t.DocumentType1)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DocumentType");
            this.Property(t => t.DocumentTypeID).HasColumnName("DocumentTypeID");
            this.Property(t => t.DocumentType1).HasColumnName("DocumentType");
        }
    }
}
