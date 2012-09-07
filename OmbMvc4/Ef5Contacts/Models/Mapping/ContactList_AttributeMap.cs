using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_AttributeMap : EntityTypeConfiguration<ContactList_Attribute>
    {
        public ContactList_AttributeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ContactList_Attribute");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AttributeTypeId).HasColumnName("AttributeTypeId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");

            // Relationships
            this.HasRequired(t => t.ContactList_AttributeType)
                .WithMany(t => t.ContactList_Attribute)
                .HasForeignKey(d => d.AttributeTypeId);

        }
    }
}
