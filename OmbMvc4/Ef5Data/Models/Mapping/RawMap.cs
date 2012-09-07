using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class RawMap : EntityTypeConfiguration<Raw>
    {
        public RawMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ReportYear, t.TipId });

            // Properties
            this.Property(t => t.ReportYear)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.TipId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Raw", "alop");
            this.Property(t => t.ReportYear).HasColumnName("ReportYear");
            this.Property(t => t.TipId).HasColumnName("TipId");
            this.Property(t => t.NetObligation).HasColumnName("NetObligation");
            this.Property(t => t.BikePedElement).HasColumnName("BikePedElement");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
