using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_OwnerMap : EntityTypeConfiguration<ContactList_Owner>
    {
        public ContactList_OwnerMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ContactList_Id, t.UserId });

            // Properties
            this.Property(t => t.ContactList_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ContactList_Owner");
            this.Property(t => t.ContactList_Id).HasColumnName("ContactList_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ContactList_Status_Id).HasColumnName("ContactList_Status_Id");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");

            // Relationships
            this.HasRequired(t => t.aspnet_Users)
                .WithMany(t => t.ContactList_Owner)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.ContactList)
                .WithMany(t => t.ContactList_Owner)
                .HasForeignKey(d => d.ContactList_Id);
            this.HasOptional(t => t.ContactList_Status)
                .WithMany(t => t.ContactList_Owner)
                .HasForeignKey(d => d.ContactList_Status_Id);

        }
    }
}
