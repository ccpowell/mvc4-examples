using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ReportOverrideMap : EntityTypeConfiguration<ReportOverride>
    {
        public ReportOverrideMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TipId, t.FundingTypeId, t.ReportFY });

            // Properties
            this.Property(t => t.TipId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FundingTypeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ReportFY)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.FederalTotal)
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("ReportOverrides", "alop");
            this.Property(t => t.TipId).HasColumnName("TipId");
            this.Property(t => t.FundingTypeId).HasColumnName("FundingTypeId");
            this.Property(t => t.ReportFY).HasColumnName("ReportFY");
            this.Property(t => t.NetObligation).HasColumnName("NetObligation");
            this.Property(t => t.FederalTotal).HasColumnName("FederalTotal");
            this.Property(t => t.ShowNetObligation).HasColumnName("ShowNetObligation");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
