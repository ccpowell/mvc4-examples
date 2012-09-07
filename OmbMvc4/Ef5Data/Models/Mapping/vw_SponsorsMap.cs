using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class vw_SponsorsMap : EntityTypeConfiguration<vw_Sponsors>
    {
        public vw_SponsorsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Firstname, t.Lastname, t.OrganizationName });

            // Properties
            this.Property(t => t.Firstname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Lastname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.OrganizationName)
                .IsRequired()
                .HasMaxLength(75);

            this.Property(t => t.Address)
                .HasMaxLength(100);

            this.Property(t => t.AddressType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("vw_Sponsors");
            this.Property(t => t.Firstname).HasColumnName("Firstname");
            this.Property(t => t.Lastname).HasColumnName("Lastname");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.OrganizationName).HasColumnName("OrganizationName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.AddressType).HasColumnName("AddressType");
        }
    }
}
