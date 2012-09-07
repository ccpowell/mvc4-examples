using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class PlanSurveyMap : EntityTypeConfiguration<PlanSurvey>
    {
        public PlanSurveyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PlanTimePeriodId, t.SurveyTimePeriodId });

            // Properties
            this.Property(t => t.PlanTimePeriodId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SurveyTimePeriodId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PlanSurvey", "Survey");
            this.Property(t => t.PlanTimePeriodId).HasColumnName("PlanTimePeriodId");
            this.Property(t => t.SurveyTimePeriodId).HasColumnName("SurveyTimePeriodId");
        }
    }
}
