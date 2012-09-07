using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProgramInstanceMap : EntityTypeConfiguration<ProgramInstance>
    {
        public ProgramInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProgramID, t.TimePeriodID });

            // Properties
            this.Property(t => t.ProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Notes)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ProgramInstance");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");
            this.Property(t => t.Current).HasColumnName("Current");
            this.Property(t => t.Pending).HasColumnName("Pending");
            this.Property(t => t.Previous).HasColumnName("Previous");
            this.Property(t => t.OpeningDate).HasColumnName("OpeningDate");
            this.Property(t => t.ClosingDate).HasColumnName("ClosingDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.StatusId).HasColumnName("StatusId");

            // Relationships
            this.HasRequired(t => t.Program)
                .WithMany(t => t.ProgramInstances)
                .HasForeignKey(d => d.ProgramID);
            this.HasOptional(t => t.Status)
                .WithMany(t => t.ProgramInstances)
                .HasForeignKey(d => d.StatusId);
            this.HasRequired(t => t.TimePeriod)
                .WithMany(t => t.ProgramInstances)
                .HasForeignKey(d => d.TimePeriodID);

        }
    }
}
