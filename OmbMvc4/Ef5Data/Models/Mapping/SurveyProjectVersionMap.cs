using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class SurveyProjectVersionMap : EntityTypeConfiguration<SurveyProjectVersion>
    {
        public SurveyProjectVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.SurveyProjectVersionID);

            // Properties
            this.Property(t => t.SurveyProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SurveyProjectVersion");
            this.Property(t => t.SurveyProjectVersionID).HasColumnName("SurveyProjectVersionID");
            this.Property(t => t.UpdateStatusID).HasColumnName("UpdateStatusID");
            this.Property(t => t.VersionStatusID).HasColumnName("VersionStatusID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.ActionStatusID).HasColumnName("ActionStatusID");
            this.Property(t => t.EndConstructionYear).HasColumnName("EndConstructionYear");
            this.Property(t => t.ConstantCost).HasColumnName("ConstantCost");
            this.Property(t => t.VisionCost).HasColumnName("VisionCost");
            this.Property(t => t.AmendedCost).HasColumnName("AmendedCost");
            this.Property(t => t.YOECost).HasColumnName("YOECost");

            // Relationships
            this.HasRequired(t => t.ProjectVersion)
                .WithOptional(t => t.SurveyProjectVersion);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.SurveyProjectVersions)
                .HasForeignKey(d => d.UpdateStatusID);
            this.HasOptional(t => t.Status1)
                .WithMany(t => t.SurveyProjectVersions1)
                .HasForeignKey(d => d.VersionStatusID);
            this.HasOptional(t => t.Status2)
                .WithMany(t => t.SurveyProjectVersions2)
                .HasForeignKey(d => d.ActionStatusID);
            this.HasOptional(t => t.TimePeriod)
                .WithMany(t => t.SurveyProjectVersions)
                .HasForeignKey(d => d.TimePeriodID);

        }
    }
}
