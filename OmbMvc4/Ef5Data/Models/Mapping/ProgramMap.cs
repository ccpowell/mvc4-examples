using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class ProgramMap : EntityTypeConfiguration<Program>
    {
        public ProgramMap()
        {
            // Primary Key
            this.HasKey(t => t.ProgramID);

            // Properties
            this.Property(t => t.Program1)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Program");
            this.Property(t => t.ProgramID).HasColumnName("ProgramID");
            this.Property(t => t.Program1).HasColumnName("Program");
        }
    }
}
