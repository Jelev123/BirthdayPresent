namespace BirthdayPresent.Infrastructure.Data.Configurations
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmployeeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
             builder
            .HasIndex(e => e.UserName)
            .IsUnique();
        }
    }
}
