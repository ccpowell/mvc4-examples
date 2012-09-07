using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class StatusTypeMap : EntityTypeConfiguration<StatusType>
    {
        public StatusTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.StatusTypeID);

            // Properties
            this.Property(t => t.StatusType1)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StatusType");
            this.Property(t => t.StatusTypeID).HasColumnName("StatusTypeID");
            this.Property(t => t.StatusType1).HasColumnName("StatusType");
            this.Property(t => t.ParentStatusTypeID).HasColumnName("ParentStatusTypeID");
        }
    }
}
