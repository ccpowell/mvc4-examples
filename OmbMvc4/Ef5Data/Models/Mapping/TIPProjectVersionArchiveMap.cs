using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProjectVersionArchiveMap : EntityTypeConfiguration<TIPProjectVersionArchive>
    {
        public TIPProjectVersionArchiveMap()
        {
            // Primary Key
            this.HasKey(t => t.TIPProjectVersionID);

            // Properties
            this.Property(t => t.TIPProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TIPID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TIPProjectVersionArchive");
            this.Property(t => t.TIPProjectVersionID).HasColumnName("TIPProjectVersionID");
            this.Property(t => t.DeferralDate).HasColumnName("DeferralDate");
            this.Property(t => t.DeferralStatusID).HasColumnName("DeferralStatusID");
            this.Property(t => t.StatusID).HasColumnName("StatusID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.VersionStatusID).HasColumnName("VersionStatusID");
            this.Property(t => t.EndConstructionYear).HasColumnName("EndConstructionYear");
            this.Property(t => t.TIPID).HasColumnName("TIPID");
        }
    }
}
