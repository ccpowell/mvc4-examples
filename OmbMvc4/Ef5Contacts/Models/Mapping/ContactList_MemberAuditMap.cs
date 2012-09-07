using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_MemberAuditMap : EntityTypeConfiguration<ContactList_MemberAudit>
    {
        public ContactList_MemberAuditMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comment)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("ContactList_MemberAudit");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ContactList_Id).HasColumnName("ContactList_Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ApproverUserId).HasColumnName("ApproverUserId");
            this.Property(t => t.ContactList_Status_Id).HasColumnName("ContactList_Status_Id");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
