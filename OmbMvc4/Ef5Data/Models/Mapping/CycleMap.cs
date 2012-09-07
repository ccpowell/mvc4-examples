using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class CycleMap : EntityTypeConfiguration<Cycle>
    {
        public CycleMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.cycle1)
                .IsRequired()
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("Cycle");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.cycle1).HasColumnName("cycle");
            this.Property(t => t.cycleGuid).HasColumnName("cycleGuid");
            this.Property(t => t.priorCycleId).HasColumnName("priorCycleId");
            this.Property(t => t.statusId).HasColumnName("statusId");

            // Relationships
            this.HasOptional(t => t.Cycle2)
                .WithMany(t => t.Cycle11)
                .HasForeignKey(d => d.priorCycleId);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.Cycles)
                .HasForeignKey(d => d.statusId);

        }
    }
}
