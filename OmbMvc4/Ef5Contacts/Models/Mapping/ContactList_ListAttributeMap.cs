using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_ListAttributeMap : EntityTypeConfiguration<ContactList_ListAttribute>
    {
        public ContactList_ListAttributeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ContactListId, t.AttributeId });

            // Properties
            this.Property(t => t.ContactListId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AttributeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("ContactList_ListAttribute");
            this.Property(t => t.ContactListId).HasColumnName("ContactListId");
            this.Property(t => t.AttributeId).HasColumnName("AttributeId");
            this.Property(t => t.Value).HasColumnName("Value");

            // Relationships
            this.HasRequired(t => t.ContactList)
                .WithMany(t => t.ContactList_ListAttribute)
                .HasForeignKey(d => d.ContactListId);
            this.HasRequired(t => t.ContactList_Attribute)
                .WithMany(t => t.ContactList_ListAttribute)
                .HasForeignKey(d => d.AttributeId);

        }
    }
}
