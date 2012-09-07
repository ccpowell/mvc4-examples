using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TimePeriodCycleMap : EntityTypeConfiguration<TimePeriodCycle>
    {
        public TimePeriodCycleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CycleId, t.TimePeriodId });

            // Properties
            this.Property(t => t.CycleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TimePeriodCycles");
            this.Property(t => t.CycleId).HasColumnName("CycleId");
            this.Property(t => t.TimePeriodId).HasColumnName("TimePeriodId");
            this.Property(t => t.ListOrder).HasColumnName("ListOrder");

            // Relationships
            this.HasRequired(t => t.Cycle)
                .WithMany(t => t.TimePeriodCycles)
                .HasForeignKey(d => d.CycleId);
            this.HasRequired(t => t.TimePeriod)
                .WithMany(t => t.TimePeriodCycles)
                .HasForeignKey(d => d.TimePeriodId);

        }
    }
}
