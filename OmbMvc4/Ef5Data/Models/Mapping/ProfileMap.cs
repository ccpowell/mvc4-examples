using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProfileMap : EntityTypeConfiguration<Profile>
    {
        public ProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfileID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Profile");
            this.Property(t => t.ProfileID).HasColumnName("ProfileID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.ProfileTypeID).HasColumnName("ProfileTypeID");
        }
    }
}
