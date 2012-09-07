using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectPoolMap : EntityTypeConfiguration<ProjectPool>
    {
        public ProjectPoolMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectPoolID);

            // Properties
            this.Property(t => t.Description)
                .HasMaxLength(255);

            this.Property(t => t.PoolName)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ProjectPool");
            this.Property(t => t.ProjectPoolID).HasColumnName("ProjectPoolID");
            this.Property(t => t.ProjectTypeID).HasColumnName("ProjectTypeID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FundingResourceID).HasColumnName("FundingResourceID");
            this.Property(t => t.PoolName).HasColumnName("PoolName");
            this.Property(t => t.BasicListVersion).HasColumnName("BasicListVersion");
            this.Property(t => t.ReportInsetTableTypeID).HasColumnName("ReportInsetTableTypeID");

            // Relationships
            this.HasOptional(t => t.FundingResource)
                .WithMany(t => t.ProjectPools)
                .HasForeignKey(d => d.FundingResourceID);
            this.HasRequired(t => t.ProgramInstance)
                .WithMany(t => t.ProjectPools)
                .HasForeignKey(d => new { d.ProgramID, d.TimePeriodID });
            this.HasOptional(t => t.ProjectType)
                .WithMany(t => t.ProjectPools)
                .HasForeignKey(d => d.ProjectTypeID);

        }
    }
}
