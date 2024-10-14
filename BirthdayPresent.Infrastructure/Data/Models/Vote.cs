namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class Vote : BaseEntity
    {
        public int VoterId { get; set; }

        public Employee Voter { get; set; }

        public int GiftId { get; set; }

        public Gift Gift { get; set; }

        public int VoteSessionId { get; set; }

        public VoteSession VoteSession { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}
