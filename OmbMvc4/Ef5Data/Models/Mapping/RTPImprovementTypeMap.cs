using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RTPImprovementTypeMap : EntityTypeConfiguration<RTPImprovementType>
    {
        public RTPImprovementTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.RTPImprovementTypeID);

            // Properties
            this.Property(t => t.RTPImprovementTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Subcode)
                .HasMaxLength(5);

            this.Property(t => t.MajorCode)
                .HasMaxLength(5);

            this.Property(t => t.Code)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("RTPImprovementType");
            this.Property(t => t.RTPImprovementTypeID).HasColumnName("RTPImprovementTypeID");
            this.Property(t => t.Subcode).HasColumnName("Subcode");
            this.Property(t => t.MajorCode).HasColumnName("MajorCode");
            this.Property(t => t.Code).HasColumnName("Code");

            // Relationships
            this.HasRequired(t => t.ImprovementType)
                .WithOptional(t => t.RTPImprovementType);

        }
    }
}
