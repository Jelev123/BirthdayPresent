namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Base;

    public class SessionStatus : BaseEntity
    {
        public SessionStatus()
        {
            this.VoteSessions = new HashSet<VoteSession>();
        }

        public string Status { get; set; }

        public ICollection<VoteSession> VoteSessions { get; set; }
    }
}
    