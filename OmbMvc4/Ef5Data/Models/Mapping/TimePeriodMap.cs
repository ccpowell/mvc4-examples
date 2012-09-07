using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TimePeriodMap : EntityTypeConfiguration<TimePeriod>
    {
        public TimePeriodMap()
        {
            // Primary Key
            this.HasKey(t => t.TimePeriodID);

            // Properties
            this.Property(t => t.TimePeriod1)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("TimePeriod");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.TimePeriod1).HasColumnName("TimePeriod");
            this.Property(t => t.TimePeriodTypeID).HasColumnName("TimePeriodTypeID");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ListOrder).HasColumnName("ListOrder");

            // Relationships
            this.HasRequired(t => t.TimePeriodType)
                .WithMany(t => t.TimePeriods)
                .HasForeignKey(d => d.TimePeriodTypeID);

        }
    }
}
