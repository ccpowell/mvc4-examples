using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProgramInstanceProjectTypeMap : EntityTypeConfiguration<ProgramInstanceProjectType>
    {
        public ProgramInstanceProjectTypeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectTypeID, t.ProgramID, t.TimePeriodID });

            // Properties
            this.Property(t => t.ProjectTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProgramID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TimePeriodID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProgramInstanceProjectType");
            this.Property(t => t.ProjectTypeID).HasColumnName("ProjectTypeID");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.TimePeriodID).HasColumnName("TimePeriodID");

            // Relationships
            this.HasRequired(t => t.ProgramInstance)
                .WithMany(t => t.ProgramInstanceProjectTypes)
                .HasForeignKey(d => new { d.ProgramID, d.TimePeriodID });
            this.HasRequired(t => t.ProjectType)
                .WithMany(t => t.ProgramInstanceProjectTypes)
                .HasForeignKey(d => d.ProjectTypeID);
            this.HasRequired(t => t.ProjectType1)
                .WithMany(t => t.ProgramInstanceProjectTypes1)
                .HasForeignKey(d => d.ProjectTypeID);

        }
    }
}
