namespace BirthdayPresent.Infrastructure.Data.Configurations
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
             builder
            .HasIndex(e => e.UserName)
            .IsUnique();
        }
    }
}
