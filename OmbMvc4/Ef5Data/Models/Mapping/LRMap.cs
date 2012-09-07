using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LRMap : EntityTypeConfiguration<LR>
    {
        public LRMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Data)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("LRS");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProjectSegmentId).HasColumnName("ProjectSegmentId");
            this.Property(t => t.LRSSchemeId).HasColumnName("LRSSchemeId");
            this.Property(t => t.Data).HasColumnName("Data");

            // Relationships
            this.HasRequired(t => t.LRSScheme)
                .WithMany(t => t.LRS)
                .HasForeignKey(d => d.LRSSchemeId);
            this.HasOptional(t => t.ProjectSegment)
                .WithMany(t => t.LRS)
                .HasForeignKey(d => d.ProjectSegmentId);

        }
    }
}
