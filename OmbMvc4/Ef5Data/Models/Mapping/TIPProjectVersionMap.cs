using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProjectVersionMap : EntityTypeConfiguration<TIPProjectVersion>
    {
        public TIPProjectVersionMap()
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
            this.ToTable("TIPProjectVersion");
            this.Property(t => t.TIPProjectVersionID).HasColumnName("TIPProjectVersionID");
            this.Property(t => t.DeferralDate).HasColumnName("DeferralDate");
            this.Property(t => t.DeferralStatusID).HasColumnName("DeferralStatusID");
            this.Property(t => t.StatusID).HasColumnName("StatusID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.VersionStatusID).HasColumnName("VersionStatusID");
            this.Property(t => t.EndConstructionYear).HasColumnName("EndConstructionYear");
            this.Property(t => t.TIPID).HasColumnName("TIPID");

            // Relationships
            this.HasRequired(t => t.ProjectVersion)
                .WithOptional(t => t.TIPProjectVersion);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.TIPProjectVersions)
                .HasForeignKey(d => d.DeferralStatusID);
            this.HasOptional(t => t.Status1)
                .WithMany(t => t.TIPProjectVersions1)
                .HasForeignKey(d => d.StatusID);
            this.HasOptional(t => t.TimePeriod)
                .WithMany(t => t.TIPProjectVersions)
                .HasForeignKey(d => d.TimePeriodID);

        }
    }
}
