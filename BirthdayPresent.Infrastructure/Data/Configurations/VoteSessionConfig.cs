namespace BirthdayPresent.Infrastructure.Data.Configurations
{
    using BirthdayPresent.Infrastructure.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VoteSessionConfig : IEntityTypeConfiguration<VoteSession>
    {
        public void Configure(EntityTypeBuilder<VoteSession> builder)
        {
            builder.HasIndex(vs => new { vs.BirthdayEmployeeId, vs.VotingYear })
             .IsUnique();

            builder
                 .HasOne(vs => vs.Initiator)
                 .WithMany(e => e.CreatedVoteSessions)
                 .HasForeignKey(vs => vs.InitiatorId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(vs => vs.BirthdayEmployee)
                .WithMany()
                .HasForeignKey(vs => vs.BirthdayEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
