using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TipImprovementTypeMap : EntityTypeConfiguration<TipImprovementType>
    {
        public TipImprovementTypeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ImprovementTypeID, t.ProjectTypeID });

            // Properties
            this.Property(t => t.ImprovementTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProjectTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TipImprovementType");
            this.Property(t => t.ImprovementTypeID).HasColumnName("ImprovementTypeID");
            this.Property(t => t.ProjectTypeID).HasColumnName("ProjectTypeID");
        }
    }
}
