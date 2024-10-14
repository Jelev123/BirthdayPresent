namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class VoteSession : BaseEntity
    {
        public VoteSession()
        {
            this.Votes = new HashSet<Vote>();
        }

        public int InitiatorId { get; set; }

        public Employee Initiator { get; set; }

        public int BirthdayEmployeeId { get; set; }

        public Employee BirthdayEmployee { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int VotingYear { get; set; }

        public int StatusId { get; set; }

        public SessionStatus Status { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
