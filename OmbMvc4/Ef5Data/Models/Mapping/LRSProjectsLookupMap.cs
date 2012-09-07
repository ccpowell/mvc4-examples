using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LRSProjectsLookupMap : EntityTypeConfiguration<LRSProjectsLookup>
    {
        public LRSProjectsLookupMap()
        {
            // Primary Key
            this.HasKey(t => new { t.LRSProjectsObjectID, t.SegmentId, t.ProjectVersionId });

            // Properties
            this.Property(t => t.LRSProjectsObjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SegmentId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectVersionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("LRSProjectsLookup");
            this.Property(t => t.LRSProjectsObjectID).HasColumnName("LRSProjectsObjectID");
            this.Property(t => t.SegmentId).HasColumnName("SegmentId");
            this.Property(t => t.ProjectVersionId).HasColumnName("ProjectVersionId");
        }
    }
}
