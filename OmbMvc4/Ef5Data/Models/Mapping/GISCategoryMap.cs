using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class GISCategoryMap : EntityTypeConfiguration<GISCategory>
    {
        public GISCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.GISCategoryID);

            // Properties
            this.Property(t => t.GISCategory1)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.Description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("GISCategory");
            this.Property(t => t.GISCategoryID).HasColumnName("GISCategoryID");
            this.Property(t => t.GISCategoryTypeID).HasColumnName("GISCategoryTypeID");
            this.Property(t => t.GISCategory1).HasColumnName("GISCategory");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.GISCategoryType)
                .WithMany(t => t.GISCategories)
                .HasForeignKey(d => d.GISCategoryTypeID);

        }
    }
}
