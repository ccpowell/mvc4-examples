using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.AddressID);

            // Properties
            this.Property(t => t.Address1)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(50);

            this.Property(t => t.FaxNumber)
                .HasMaxLength(50);

            this.Property(t => t.CellNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Address");
            this.Property(t => t.AddressID).HasColumnName("AddressID");
            this.Property(t => t.AddressTypeID).HasColumnName("AddressTypeID");
            this.Property(t => t.Address1).HasColumnName("Address");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.OrganizationID).HasColumnName("OrganizationID");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.FaxNumber).HasColumnName("FaxNumber");
            this.Property(t => t.CellNumber).HasColumnName("CellNumber");

            // Relationships
            this.HasRequired(t => t.AddressType)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.AddressTypeID);
            this.HasOptional(t => t.Organization)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.OrganizationID);
            this.HasOptional(t => t.Person)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
