using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ReportProjectVersionSortingMap : EntityTypeConfiguration<ReportProjectVersionSorting>
    {
        public ReportProjectVersionSortingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ReportId, t.ProjectVersionId });

            // Properties
            this.Property(t => t.ReportId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectVersionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ReportProjectVersionSorting");
            this.Property(t => t.ReportId).HasColumnName("ReportId");
            this.Property(t => t.ProjectVersionId).HasColumnName("ProjectVersionId");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");

            // Relationships
            this.HasRequired(t => t.ProjectVersion)
                .WithMany(t => t.ReportProjectVersionSortings)
                .HasForeignKey(d => d.ProjectVersionId);
            this.HasRequired(t => t.Report1)
                .WithMany(t => t.ReportProjectVersionSortings)
                .HasForeignKey(d => d.ReportId);

        }
    }
}
