using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class VersionMap : EntityTypeConfiguration<Version>
    {
        public VersionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Uid, t.SessionId, t.PropertyName, t.Current, t.DateCreated });

            // Properties
            this.Property(t => t.PropertyName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Prior)
                .HasMaxLength(4000);

            this.Property(t => t.Current)
                .IsRequired()
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("Version");
            this.Property(t => t.Uid).HasColumnName("Uid");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.PropertyName).HasColumnName("PropertyName");
            this.Property(t => t.Prior).HasColumnName("Prior");
            this.Property(t => t.Current).HasColumnName("Current");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
