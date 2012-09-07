using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ImprovementTypeMap : EntityTypeConfiguration<ImprovementType>
    {
        public ImprovementTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ImprovementTypeID);

            // Properties
            this.Property(t => t.ImprovementType1)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Details)
                .HasMaxLength(75);

            this.Property(t => t.ShortName)
                .HasMaxLength(50);

            this.Property(t => t.Notes)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("ImprovementType");
            this.Property(t => t.ImprovementTypeID).HasColumnName("ImprovementTypeID");
            this.Property(t => t.ImprovementType1).HasColumnName("ImprovementType");
            this.Property(t => t.ProjectTypeID).HasColumnName("ProjectTypeID");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.ModeID).HasColumnName("ModeID");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Construction).HasColumnName("Construction");
            this.Property(t => t.RSPCodeID).HasColumnName("RSPCodeID");

            // Relationships
            this.HasMany(t => t.ProgramInstances)
                .WithMany(t => t.ImprovementTypes)
                .Map(m =>
                    {
                        m.ToTable("ProgramInstanceImprovementType");
                        m.MapLeftKey("ImprovementTypeID");
                        m.MapRightKey("ProgramID", "TimePeriodID");
                    });

            this.HasOptional(t => t.ProjectType)
                .WithMany(t => t.ImprovementTypes)
                .HasForeignKey(d => d.ProjectTypeID);

        }
    }
}
