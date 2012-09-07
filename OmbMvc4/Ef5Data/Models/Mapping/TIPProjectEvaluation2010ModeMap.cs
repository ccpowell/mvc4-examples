using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProjectEvaluation2010ModeMap : EntityTypeConfiguration<TIPProjectEvaluation2010Mode>
    {
        public TIPProjectEvaluation2010ModeMap()
        {
            // Primary Key
            this.HasKey(t => t.ModeID);

            // Properties
            this.Property(t => t.Mode)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TIPProjectEvaluation2010Mode");
            this.Property(t => t.ModeID).HasColumnName("ModeID");
            this.Property(t => t.Mode).HasColumnName("Mode");
        }
    }
}
