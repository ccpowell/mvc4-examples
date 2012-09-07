using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class ContactList_UserCacheMap : EntityTypeConfiguration<ContactList_UserCache>
    {
        public ContactList_UserCacheMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.LastName)
                .HasMaxLength(256);

            this.Property(t => t.FirstName)
                .HasMaxLength(256);

            this.Property(t => t.Organization)
                .HasMaxLength(256);

            this.Property(t => t.Title)
                .HasMaxLength(256);

            this.Property(t => t.PrimaryContact)
                .HasMaxLength(256);

            this.Property(t => t.LoweredEmail)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.LoweredBusinessEmail)
                .HasMaxLength(256);

            this.Property(t => t.LoweredApplicationName)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ContactList_UserCache");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.Organization).HasColumnName("Organization");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.PrimaryContact).HasColumnName("PrimaryContact");
            this.Property(t => t.LoweredEmail).HasColumnName("LoweredEmail");
            this.Property(t => t.LoweredBusinessEmail).HasColumnName("LoweredBusinessEmail");
            this.Property(t => t.LoweredApplicationName).HasColumnName("LoweredApplicationName");
        }
    }
}
