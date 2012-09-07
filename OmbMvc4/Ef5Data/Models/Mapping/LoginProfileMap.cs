using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class LoginProfileMap : EntityTypeConfiguration<LoginProfile>
    {
        public LoginProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.LoginProfileID);

            // Properties
            this.Property(t => t.LoginProfileID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LoginProfile");
            this.Property(t => t.LoginProfileID).HasColumnName("LoginProfileID");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.ChangedPasswordDate).HasColumnName("ChangedPasswordDate");
        }
    }
}
