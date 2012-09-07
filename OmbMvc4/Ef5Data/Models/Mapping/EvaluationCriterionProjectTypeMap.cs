using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class EvaluationCriterionProjectTypeMap : EntityTypeConfiguration<EvaluationCriterionProjectType>
    {
        public EvaluationCriterionProjectTypeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EvaluationCriterionID, t.ProjectType });

            // Properties
            this.Property(t => t.EvaluationCriterionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectType)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("EvaluationCriterionProjectType");
            this.Property(t => t.EvaluationCriterionID).HasColumnName("EvaluationCriterionID");
            this.Property(t => t.ProjectType).HasColumnName("ProjectType");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
        }
    }
}
