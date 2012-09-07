using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class AddressTypeMap : EntityTypeConfiguration<AddressType>
    {
        public AddressTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.AddressTypeID);

            // Properties
            this.Property(t => t.AddressType1)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AddressType");
            this.Property(t => t.AddressTypeID).HasColumnName("AddressTypeID");
            this.Property(t => t.AddressType1).HasColumnName("AddressType");
        }
    }
}
