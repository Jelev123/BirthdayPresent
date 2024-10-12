namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class Vote : BaseEntity
    {
        public string VoterId { get; set; }

        public User Voter { get; set; }

        public int GiftId { get; set; }

        public Gift Gift { get; set; }

        public int VoteSessionId { get; set; }

        public VoteSession VoteSession { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}
