using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class StatusMap : EntityTypeConfiguration<Status>
    {
        public StatusMap()
        {
            // Primary Key
            this.HasKey(t => t.StatusID);

            // Properties
            this.Property(t => t.Status1)
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Status");
            this.Property(t => t.StatusID).HasColumnName("StatusID");
            this.Property(t => t.StatusTypeID).HasColumnName("StatusTypeID");
            this.Property(t => t.Status1).HasColumnName("Status");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
