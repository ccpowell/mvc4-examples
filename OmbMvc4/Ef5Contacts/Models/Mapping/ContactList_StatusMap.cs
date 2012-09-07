using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_StatusMap : EntityTypeConfiguration<ContactList_Status>
    {
        public ContactList_StatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ContactList_Status");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.StatusTypeId).HasColumnName("StatusTypeId");

            // Relationships
            this.HasRequired(t => t.StatusType)
                .WithMany(t => t.ContactList_Status)
                .HasForeignKey(d => d.StatusTypeId);

        }
    }
}
