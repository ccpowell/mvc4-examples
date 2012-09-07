using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectVersionReportingMap : EntityTypeConfiguration<ProjectVersionReporting>
    {
        public ProjectVersionReportingMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectVersionID);

            // Properties
            this.Property(t => t.ProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProjectVersionReporting");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.AmendmentOrder).HasColumnName("AmendmentOrder");
            this.Property(t => t.PageNumber).HasColumnName("PageNumber");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
        }
    }
}
