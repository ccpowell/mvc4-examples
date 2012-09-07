using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactListMap : EntityTypeConfiguration<ContactList>
    {
        public ContactListMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ContactList");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsPrivate).HasColumnName("IsPrivate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
