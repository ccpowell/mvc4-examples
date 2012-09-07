using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectModelCodingMap : EntityTypeConfiguration<ProjectModelCoding>
    {
        public ProjectModelCodingMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectModelCodingID);

            // Properties
            this.Property(t => t.ScenarioNameID)
                .HasMaxLength(50);

            this.Property(t => t.Temp_OldEndConstr)
                .HasMaxLength(50);

            this.Property(t => t.Temp_RegionRank)
                .HasMaxLength(50);

            this.Property(t => t.Temp_TIPSelectNum)
                .HasMaxLength(50);

            this.Property(t => t.Temp_UniqueID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProjectModelCoding");
            this.Property(t => t.ProjectModelCodingID).HasColumnName("ProjectModelCodingID");
            this.Property(t => t.ProjectSegmentID).HasColumnName("ProjectSegmentID");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.ScenarioNameID).HasColumnName("ScenarioNameID");
            this.Property(t => t.CodingStatusID).HasColumnName("CodingStatusID");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Temp_OldEndConstr).HasColumnName("Temp_OldEndConstr");
            this.Property(t => t.Temp_RegionRank).HasColumnName("Temp_RegionRank");
            this.Property(t => t.Temp_TIPSelectNum).HasColumnName("Temp_TIPSelectNum");
            this.Property(t => t.Temp_UniqueID).HasColumnName("Temp_UniqueID");

            // Relationships
            this.HasOptional(t => t.ProjectSegment)
                .WithMany(t => t.ProjectModelCodings)
                .HasForeignKey(d => d.ProjectSegmentID);
            this.HasOptional(t => t.ProjectVersion)
                .WithMany(t => t.ProjectModelCodings)
                .HasForeignKey(d => d.ProjectVersionID);

        }
    }
}
