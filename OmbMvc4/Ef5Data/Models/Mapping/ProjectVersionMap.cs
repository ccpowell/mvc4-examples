using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectVersionMap : EntityTypeConfiguration<ProjectVersion>
    {
        public ProjectVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectVersionID);

            // Properties
            this.Property(t => t.Limits)
                .HasMaxLength(255);

            this.Property(t => t.BeginAt)
                .HasMaxLength(50);

            this.Property(t => t.EndAt)
                .HasMaxLength(50);

            this.Property(t => t.FacilityName)
                .HasMaxLength(75);

            this.Property(t => t.ProjectName)
                .HasMaxLength(100);

            this.Property(t => t.ProjectType)
                .HasMaxLength(20);

            this.Property(t => t.STIPId)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ProjectVersion");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.PoolID).HasColumnName("PoolID");
            this.Property(t => t.PoolMasterVersionID).HasColumnName("PoolMasterVersionID");
            this.Property(t => t.Limits).HasColumnName("Limits");
            this.Property(t => t.BeginAt).HasColumnName("BeginAt");
            this.Property(t => t.EndAt).HasColumnName("EndAt");
            this.Property(t => t.FacilityName).HasColumnName("FacilityName");
            this.Property(t => t.AmendmentTypeID).HasColumnName("AmendmentTypeID");
            this.Property(t => t.AmendmentReasonID).HasColumnName("AmendmentReasonID");
            this.Property(t => t.Temp_PreviousScopeID).HasColumnName("Temp_PreviousScopeID");
            this.Property(t => t.PreviousProjectVersionID).HasColumnName("PreviousProjectVersionID");
            this.Property(t => t.AmendmentStatusID).HasColumnName("AmendmentStatusID");
            this.Property(t => t.AmendmentDate).HasColumnName("AmendmentDate");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.AmendmentFundingTypeID).HasColumnName("AmendmentFundingTypeID");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.Scope).HasColumnName("Scope");
            this.Property(t => t.SponsorContactID).HasColumnName("SponsorContactID");
            this.Property(t => t.SponsorNotes).HasColumnName("SponsorNotes");
            this.Property(t => t.DRCOGNotes).HasColumnName("DRCOGNotes");
            this.Property(t => t.LocationMapID).HasColumnName("LocationMapID");
            this.Property(t => t.CrossSectionMapID___deprecating).HasColumnName("CrossSectionMapID - deprecating");
            this.Property(t => t.AmendmentCharacter).HasColumnName("AmendmentCharacter");
            this.Property(t => t.AmendmentReason).HasColumnName("AmendmentReason");
            this.Property(t => t.ModelingStatusID).HasColumnName("ModelingStatusID");
            this.Property(t => t.ShapeFileID).HasColumnName("ShapeFileID");
            this.Property(t => t.BeginConstructionYear).HasColumnName("BeginConstructionYear");
            this.Property(t => t.ProjectType).HasColumnName("ProjectType");
            this.Property(t => t.CDOTRegionId).HasColumnName("CDOTRegionId");
            this.Property(t => t.STIPId).HasColumnName("STIPId");
            this.Property(t => t.AffectedProjectDelaysLocationId).HasColumnName("AffectedProjectDelaysLocationId");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.ProjectVersions)
                .HasForeignKey(d => d.AffectedProjectDelaysLocationId);
            this.HasOptional(t => t.Category1)
                .WithMany(t => t.ProjectVersions1)
                .HasForeignKey(d => d.CDOTRegionId);
            this.HasOptional(t => t.ProjectPool)
                .WithMany(t => t.ProjectVersions)
                .HasForeignKey(d => d.PoolID);
            this.HasOptional(t => t.ProjectVersion2)
                .WithMany(t => t.ProjectVersion1)
                .HasForeignKey(d => d.PoolMasterVersionID);
            this.HasOptional(t => t.ProjectVersion3)
                .WithMany(t => t.ProjectVersion11)
                .HasForeignKey(d => d.PreviousProjectVersionID);

        }
    }
}
