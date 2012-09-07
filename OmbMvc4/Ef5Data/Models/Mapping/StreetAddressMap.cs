using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class StreetAddressMap : EntityTypeConfiguration<StreetAddress>
    {
        public StreetAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.StreetAddressID);

            // Properties
            this.Property(t => t.StreetAddressID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Address2)
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.State)
                .HasMaxLength(10);

            this.Property(t => t.ZipCode)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("StreetAddress");
            this.Property(t => t.StreetAddressID).HasColumnName("StreetAddressID");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");

            // Relationships
            this.HasRequired(t => t.Address)
                .WithOptional(t => t.StreetAddress);

        }
    }
}
