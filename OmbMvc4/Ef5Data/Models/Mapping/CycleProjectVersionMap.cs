using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CycleProjectVersionMap : EntityTypeConfiguration<CycleProjectVersion>
    {
        public CycleProjectVersionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectVersionId, t.CycleId });

            // Properties
            this.Property(t => t.ProjectVersionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CycleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CycleProjectVersion", "RTP");
            this.Property(t => t.ProjectVersionId).HasColumnName("ProjectVersionId");
            this.Property(t => t.CycleId).HasColumnName("CycleId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            // Relationships
            this.HasRequired(t => t.Cycle)
                .WithMany(t => t.CycleProjectVersions)
                .HasForeignKey(d => d.CycleId);
            this.HasRequired(t => t.RTPProjectVersion)
                .WithMany(t => t.CycleProjectVersions)
                .HasForeignKey(d => d.ProjectVersionId);

        }
    }
}
