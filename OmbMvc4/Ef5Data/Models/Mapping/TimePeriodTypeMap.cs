using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TimePeriodTypeMap : EntityTypeConfiguration<TimePeriodType>
    {
        public TimePeriodTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.TimePeriodTypeID);

            // Properties
            this.Property(t => t.TimePeriodType1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TimePeriodType");
            this.Property(t => t.TimePeriodTypeID).HasColumnName("TimePeriodTypeID");
            this.Property(t => t.TimePeriodType1).HasColumnName("TimePeriodType");
        }
    }
}
