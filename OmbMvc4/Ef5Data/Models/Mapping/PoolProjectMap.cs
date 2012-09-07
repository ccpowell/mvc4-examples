using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class PoolProjectMap : EntityTypeConfiguration<PoolProject>
    {
        public PoolProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.PoolProjectID);

            // Properties
            this.Property(t => t.ProjectName)
                .HasMaxLength(255);

            this.Property(t => t.Description)
                .HasMaxLength(75);

            this.Property(t => t.BeginAt)
                .HasMaxLength(75);

            this.Property(t => t.EndAt)
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("PoolProject");
            this.Property(t => t.PoolProjectID).HasColumnName("PoolProjectID");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.BeginAt).HasColumnName("BeginAt");
            this.Property(t => t.EndAt).HasColumnName("EndAt");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.PoolMasterVersionID).HasColumnName("PoolMasterVersionID");

            // Relationships
            this.HasOptional(t => t.ProjectVersion)
                .WithMany(t => t.PoolProjects)
                .HasForeignKey(d => d.PoolMasterVersionID);

        }
    }
}
