using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectCDOTDataMap : EntityTypeConfiguration<ProjectCDOTData>
    {
        public ProjectCDOTDataMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectVersionID);

            // Properties
            this.Property(t => t.ProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RouteSegment)
                .HasMaxLength(8);

            this.Property(t => t.STIPID)
                .HasMaxLength(20);

            this.Property(t => t.STIPProjectDivision)
                .HasMaxLength(15);

            this.Property(t => t.TPRAbbr)
                .HasMaxLength(3);

            this.Property(t => t.InvestmentCategory)
                .HasMaxLength(50);

            this.Property(t => t.CorridorID)
                .HasMaxLength(50);

            this.Property(t => t.CDOTProjectNumber)
                .HasMaxLength(15);

            this.Property(t => t.CMSNumber)
                .HasMaxLength(6);

            // Table & Column Mappings
            this.ToTable("ProjectCDOTData");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.CommDistrict).HasColumnName("CommDistrict");
            this.Property(t => t.RouteSegment).HasColumnName("RouteSegment");
            this.Property(t => t.BeginMilePost).HasColumnName("BeginMilePost");
            this.Property(t => t.EndMilePost).HasColumnName("EndMilePost");
            this.Property(t => t.STIPID).HasColumnName("STIPID");
            this.Property(t => t.STIPProjectDivision).HasColumnName("STIPProjectDivision");
            this.Property(t => t.CDOTProjectManager).HasColumnName("CDOTProjectManager");
            this.Property(t => t.TPRAbbr).HasColumnName("TPRAbbr");
            this.Property(t => t.TPRID).HasColumnName("TPRID");
            this.Property(t => t.LRPNumber).HasColumnName("LRPNumber");
            this.Property(t => t.InvestmentCategory).HasColumnName("InvestmentCategory");
            this.Property(t => t.CorridorID).HasColumnName("CorridorID");
            this.Property(t => t.CDOTProjectNumber).HasColumnName("CDOTProjectNumber");
            this.Property(t => t.SubAccount).HasColumnName("SubAccount");
            this.Property(t => t.ConstructionRE).HasColumnName("ConstructionRE");
            this.Property(t => t.CMSNumber).HasColumnName("CMSNumber");
            this.Property(t => t.ScheduledADDate).HasColumnName("ScheduledADDate");
            this.Property(t => t.ProjectStage).HasColumnName("ProjectStage");
            this.Property(t => t.ProjectStageDate).HasColumnName("ProjectStageDate");
            this.Property(t => t.ConstructionDate).HasColumnName("ConstructionDate");
            this.Property(t => t.ProjectClosed).HasColumnName("ProjectClosed");
            this.Property(t => t.Notes).HasColumnName("Notes");

            // Relationships
            this.HasRequired(t => t.ProjectVersion)
                .WithOptional(t => t.ProjectCDOTData);

        }
    }
}
