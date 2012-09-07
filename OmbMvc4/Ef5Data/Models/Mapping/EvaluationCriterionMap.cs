using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class EvaluationCriterionMap : EntityTypeConfiguration<EvaluationCriterion>
    {
        public EvaluationCriterionMap()
        {
            // Primary Key
            this.HasKey(t => t.EvaluationCriterionID);

            // Properties
            this.Property(t => t.EvaluationCriterionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.EvaluationCriterion1)
                .HasMaxLength(255);

            this.Property(t => t.Code)
                .HasMaxLength(25);

            this.Property(t => t.Path)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EvaluationCriterion");
            this.Property(t => t.EvaluationCriterionID).HasColumnName("EvaluationCriterionID");
            this.Property(t => t.EvaluationCriterion1).HasColumnName("EvaluationCriterion");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
        }
    }
}
