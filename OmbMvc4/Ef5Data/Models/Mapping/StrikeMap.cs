using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class StrikeMap : EntityTypeConfiguration<Strike>
    {
        public StrikeMap()
        {
            // Primary Key
            this.HasKey(t => t.StrikeID);

            // Properties
            this.Property(t => t.StatusOther)
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("Strike");
            this.Property(t => t.StrikeID).HasColumnName("StrikeID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.StrikeReasonID).HasColumnName("StrikeReasonID");
            this.Property(t => t.SponsorReaction).HasColumnName("SponsorReaction");
            this.Property(t => t.DRCOGAction).HasColumnName("DRCOGAction");
            this.Property(t => t.EffectiveDate).HasColumnName("EffectiveDate");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.StatusID).HasColumnName("StatusID");
            this.Property(t => t.StatusOther).HasColumnName("StatusOther");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.Strikes)
                .HasForeignKey(d => d.StrikeReasonID);
            this.HasRequired(t => t.Project)
                .WithMany(t => t.Strikes)
                .HasForeignKey(d => d.ProjectID);
            this.HasRequired(t => t.Status)
                .WithMany(t => t.Strikes)
                .HasForeignKey(d => d.StatusID);

        }
    }
}
