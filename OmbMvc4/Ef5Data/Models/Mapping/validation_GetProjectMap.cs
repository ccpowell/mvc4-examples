using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class validation_GetProjectMap : EntityTypeConfiguration<validation_GetProject>
    {
        public validation_GetProjectMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProjectID, t.COGID });

            // Properties
            this.Property(t => t.ProjectID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.COGID)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ImprovementType)
                .HasMaxLength(100);

            this.Property(t => t.Selector)
                .HasMaxLength(75);

            this.Property(t => t.Route)
                .HasMaxLength(75);

            this.Property(t => t.Administrative_Level)
                .HasMaxLength(75);

            this.Property(t => t.Transportation_Type)
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("validation_GetProject");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.COGID).HasColumnName("COGID");
            this.Property(t => t.ImprovementType).HasColumnName("ImprovementType");
            this.Property(t => t.Selector).HasColumnName("Selector");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.RegionalSignificance).HasColumnName("RegionalSignificance");
            this.Property(t => t.Route).HasColumnName("Route");
            this.Property(t => t.Administrative_Level).HasColumnName("Administrative Level");
            this.Property(t => t.Transportation_Type).HasColumnName("Transportation Type");
        }
    }
}
