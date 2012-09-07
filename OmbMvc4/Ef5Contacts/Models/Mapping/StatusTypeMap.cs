using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class StatusTypeMap : EntityTypeConfiguration<StatusType>
    {
        public StatusTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("StatusType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
