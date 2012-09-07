using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProcessedMap : EntityTypeConfiguration<Processed>
    {
        public ProcessedMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ReportId, t.TipId });

            // Properties
            this.Property(t => t.TipId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Processed", "alop");
            this.Property(t => t.ReportId).HasColumnName("ReportId");
            this.Property(t => t.TipId).HasColumnName("TipId");
            this.Property(t => t.NetObligation).HasColumnName("NetObligation");
            this.Property(t => t.BikePedElement).HasColumnName("BikePedElement");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            this.HasRequired(t => t.Report)
                .WithMany(t => t.Processeds)
                .HasForeignKey(d => d.ReportId);

        }
    }
}
