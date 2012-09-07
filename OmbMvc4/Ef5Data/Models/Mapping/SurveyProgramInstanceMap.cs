using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class SurveyProgramInstanceMap : EntityTypeConfiguration<SurveyProgramInstance>
    {
        public SurveyProgramInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SurveyProgramID, t.TimePeriodID });

            // Properties
            this.Property(t => t.SurveyProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SurveyProgramInstance");
            this.Property(t => t.SurveyProgramID).HasColumnName("SurveyProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.PreviousTimePeriodID).HasColumnName("PreviousTimePeriodID");
            this.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");

            // Relationships
            this.HasRequired(t => t.ProgramInstance)
                .WithOptional(t => t.SurveyProgramInstance);

        }
    }
}
