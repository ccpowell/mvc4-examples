using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectID);

            // Properties
            this.Property(t => t.COGID)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Project");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.COGID).HasColumnName("COGID");
            this.Property(t => t.ImprovementTypeID).HasColumnName("ImprovementTypeID");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.RegionalSignificance).HasColumnName("RegionalSignificance");
            this.Property(t => t.SelectorID).HasColumnName("SelectorID");
            this.Property(t => t.RouteID).HasColumnName("RouteID");
            this.Property(t => t.AdministrativeLevelID).HasColumnName("AdministrativeLevelID");
            this.Property(t => t.TransportationTypeID).HasColumnName("TransportationTypeID");
            this.Property(t => t.SubmittedByID).HasColumnName("SubmittedByID");
            this.Property(t => t.ComplexProject).HasColumnName("ComplexProject");
            this.Property(t => t.temp_projectIDfromtip2010).HasColumnName("temp_projectIDfromtip2010");

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.SelectorID);
            this.HasOptional(t => t.Category1)
                .WithMany(t => t.Projects1)
                .HasForeignKey(d => d.AdministrativeLevelID);
            this.HasOptional(t => t.Category2)
                .WithMany(t => t.Projects2)
                .HasForeignKey(d => d.TransportationTypeID);
            this.HasOptional(t => t.Person)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.SubmittedByID);

        }
    }
}
