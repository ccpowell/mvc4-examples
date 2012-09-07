using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class Report1Map : EntityTypeConfiguration<Report1>
    {
        public Report1Map()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TimePeriodId).HasColumnName("TimePeriodId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.TimePeriod)
                .WithMany(t => t.Report1)
                .HasForeignKey(d => d.TimePeriodId);

        }
    }
}
