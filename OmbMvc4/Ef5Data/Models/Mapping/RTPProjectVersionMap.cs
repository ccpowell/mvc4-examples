using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RTPProjectVersionMap : EntityTypeConfiguration<RTPProjectVersion>
    {
        public RTPProjectVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.RTPProjectVersionID);

            // Properties
            this.Property(t => t.RTPProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ShortDescription)
                .HasMaxLength(256);

            this.Property(t => t.Temp_RTPSubcategory)
                .HasMaxLength(50);

            this.Property(t => t.AmendmentReason)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("RTPProjectVersion");
            this.Property(t => t.RTPProjectVersionID).HasColumnName("RTPProjectVersionID");
            this.Property(t => t.PlanID).HasColumnName("PlanID");
            this.Property(t => t.StageID).HasColumnName("StageID");
            this.Property(t => t.CycleId).HasColumnName("CycleId");
            this.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            this.Property(t => t.ConstantCost).HasColumnName("ConstantCost");
            this.Property(t => t.VisionCost).HasColumnName("VisionCost");
            this.Property(t => t.YOECost).HasColumnName("YOECost");
            this.Property(t => t.RTPCategoryID).HasColumnName("RTPCategoryID");
            this.Property(t => t.Temp_RTPSubcategory).HasColumnName("Temp_RTPSubcategory");
            this.Property(t => t.StatusID).HasColumnName("StatusID");
            this.Property(t => t.VersionStatusID).HasColumnName("VersionStatusID");
            this.Property(t => t.PlanTypeID).HasColumnName("PlanTypeID");
            this.Property(t => t.RevisedConstantCost).HasColumnName("RevisedConstantCost");
            this.Property(t => t.AmendmentStatusID).HasColumnName("AmendmentStatusID");
            this.Property(t => t.AmendmentReason).HasColumnName("AmendmentReason");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.RTPProjectVersions)
                .HasForeignKey(d => d.PlanTypeID);
            this.HasOptional(t => t.Category1)
                .WithMany(t => t.RTPProjectVersions1)
                .HasForeignKey(d => d.RTPCategoryID);
            this.HasOptional(t => t.Category2)
                .WithMany(t => t.RTPProjectVersions2)
                .HasForeignKey(d => d.StageID);
            this.HasOptional(t => t.Cycle)
                .WithMany(t => t.RTPProjectVersions)
                .HasForeignKey(d => d.CycleId);
            this.HasRequired(t => t.ProjectVersion)
                .WithOptional(t => t.RTPProjectVersion);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.RTPProjectVersions)
                .HasForeignKey(d => d.StatusID);
            this.HasOptional(t => t.TimePeriod)
                .WithMany(t => t.RTPProjectVersions)
                .HasForeignKey(d => d.PlanID);

        }
    }
}
