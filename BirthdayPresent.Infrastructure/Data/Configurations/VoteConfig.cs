namespace BirthdayPresent.Infrastructure.Data.Configurations
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VoteConfig : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder
            .HasOne(v => v.Voter)
            .WithMany(e => e.Votes)
            .HasForeignKey(vs => vs.VoterId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
