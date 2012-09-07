using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            // Primary Key
            this.HasKey(t => t.PersonID);

            // Properties
            this.Property(t => t.Firstname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MiddleInitial)
                .HasMaxLength(5);

            this.Property(t => t.Lastname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Division)
                .HasMaxLength(75);

            this.Property(t => t.Title)
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.SponsorKey)
                .HasMaxLength(12);

            // Table & Column Mappings
            this.ToTable("Person");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.PersonGUID).HasColumnName("PersonGUID");
            this.Property(t => t.Firstname).HasColumnName("Firstname");
            this.Property(t => t.MiddleInitial).HasColumnName("MiddleInitial");
            this.Property(t => t.Lastname).HasColumnName("Lastname");
            this.Property(t => t.Division).HasColumnName("Division");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Temp_PreviousContactID).HasColumnName("Temp_PreviousContactID");
            this.Property(t => t.SponsorKey).HasColumnName("SponsorKey");
        }
    }
}
