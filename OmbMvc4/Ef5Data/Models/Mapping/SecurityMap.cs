using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class SecurityMap : EntityTypeConfiguration<Security>
    {
        public SecurityMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PersonID, t.RandomNumber });

            // Properties
            this.Property(t => t.PersonID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RandomNumber)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Security");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.RandomNumber).HasColumnName("RandomNumber");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
        }
    }
}
