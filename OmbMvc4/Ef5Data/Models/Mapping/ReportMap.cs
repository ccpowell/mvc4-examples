using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ReportMap : EntityTypeConfiguration<Report>
    {
        public ReportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ReportYear)
                .IsRequired()
                .HasMaxLength(6);

            // Table & Column Mappings
            this.ToTable("Report", "alop");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ReportYear).HasColumnName("ReportYear");
        }
    }
}
