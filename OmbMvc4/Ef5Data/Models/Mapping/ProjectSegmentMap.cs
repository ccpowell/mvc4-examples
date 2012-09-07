using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectSegmentMap : EntityTypeConfiguration<ProjectSegment>
    {
        public ProjectSegmentMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectSegmentID);

            // Properties
            this.Property(t => t.FacilityName)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.StartAt)
                .HasMaxLength(50);

            this.Property(t => t.EndAt)
                .HasMaxLength(50);

            this.Property(t => t.LRSLinkage)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProjectSegment");
            this.Property(t => t.ProjectSegmentID).HasColumnName("ProjectSegmentID");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.ImprovementTypeID).HasColumnName("ImprovementTypeID");
            this.Property(t => t.ModelingFacilityTypeID).HasColumnName("ModelingFacilityTypeID");
            this.Property(t => t.PlanFacilityTypeID).HasColumnName("PlanFacilityTypeID");
            this.Property(t => t.NetworkID).HasColumnName("NetworkID");
            this.Property(t => t.OpenYear).HasColumnName("OpenYear");
            this.Property(t => t.FacilityName).HasColumnName("FacilityName");
            this.Property(t => t.StartAt).HasColumnName("StartAt");
            this.Property(t => t.EndAt).HasColumnName("EndAt");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.LanesBase).HasColumnName("LanesBase");
            this.Property(t => t.LanesFuture).HasColumnName("LanesFuture");
            this.Property(t => t.SpacesFuture).HasColumnName("SpacesFuture");
            this.Property(t => t.VehiclesFuture).HasColumnName("VehiclesFuture");
            this.Property(t => t.LRSObjectID).HasColumnName("LRSObjectID");
            this.Property(t => t.AssignmentStatusID).HasColumnName("AssignmentStatusID");
            this.Property(t => t.LRSLinkage).HasColumnName("LRSLinkage");
            this.Property(t => t.LRSLinkageStatusID).HasColumnName("LRSLinkageStatusID");
            this.Property(t => t.NeedLocationMap).HasColumnName("NeedLocationMap");
            this.Property(t => t.Temp_PreviousImproveID).HasColumnName("Temp_PreviousImproveID");
            this.Property(t => t.ModelingCheck).HasColumnName("ModelingCheck");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.SpacesBase).HasColumnName("SpacesBase");

            // Relationships
            this.HasOptional(t => t.GISCategory)
                .WithMany(t => t.ProjectSegments)
                .HasForeignKey(d => d.ModelingFacilityTypeID);
            this.HasOptional(t => t.GISCategory1)
                .WithMany(t => t.ProjectSegments1)
                .HasForeignKey(d => d.PlanFacilityTypeID);
            this.HasOptional(t => t.ImprovementType)
                .WithMany(t => t.ProjectSegments)
                .HasForeignKey(d => d.ImprovementTypeID);
            this.HasOptional(t => t.Network)
                .WithMany(t => t.ProjectSegments)
                .HasForeignKey(d => d.NetworkID);
            this.HasRequired(t => t.ProjectVersion)
                .WithMany(t => t.ProjectSegments)
                .HasForeignKey(d => d.ProjectVersionID);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.ProjectSegments)
                .HasForeignKey(d => d.AssignmentStatusID);
            this.HasOptional(t => t.Status1)
                .WithMany(t => t.ProjectSegments1)
                .HasForeignKey(d => d.LRSLinkageStatusID);

        }
    }
}
