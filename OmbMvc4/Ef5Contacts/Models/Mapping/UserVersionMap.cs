using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Contacts.Models.Mapping
{
    public class UserVersionMap : EntityTypeConfiguration<UserVersion>
    {
        public UserVersionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Users_UserId, t.Version_Uid });

            // Properties
            // Table & Column Mappings
            this.ToTable("UserVersion");
            this.Property(t => t.Users_UserId).HasColumnName("Users_UserId");
            this.Property(t => t.Version_Uid).HasColumnName("Version_Uid");
        }
    }
}
