namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class VoteSession : BaseEntity
    {
        public VoteSession()
        {
            this.Votes = new HashSet<Vote>();
        }

        public string InitiatorId { get; set; }

        public User Initiator { get; set; }

        public string BirthdayEmployeeId { get; set; }

        public User BirthdayEmployee { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime VotingYear { get; set; }

        public string Status { get; set; } = "Active";

        public ICollection<Vote> Votes { get; set; }
    }
}
