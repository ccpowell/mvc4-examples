using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProjectTypeMap : EntityTypeConfiguration<ProjectType>
    {
        public ProjectTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectTypeID);

            // Properties
            this.Property(t => t.ProjectType1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ProjectType");
            this.Property(t => t.ProjectTypeID).HasColumnName("ProjectTypeID");
            this.Property(t => t.ParentTypeID).HasColumnName("ParentTypeID");
            this.Property(t => t.PolicyGroupID).HasColumnName("PolicyGroupID");
            this.Property(t => t.ProjectType1).HasColumnName("ProjectType");

            // Relationships
            this.HasOptional(t => t.ProjectType2)
                .WithMany(t => t.ProjectType11)
                .HasForeignKey(d => d.PolicyGroupID);
            this.HasOptional(t => t.ProjectType3)
                .WithMany(t => t.ProjectType12)
                .HasForeignKey(d => d.ParentTypeID);

        }
    }
}
