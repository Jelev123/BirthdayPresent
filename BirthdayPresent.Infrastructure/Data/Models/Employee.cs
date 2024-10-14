namespace BirthdayPresent.Infrastructure.Data.Models
{
    using BirthdayPresent.Infrastructure.Data.Models.Interfaces.Base;
    using Microsoft.AspNetCore.Identity;

    public class Employee : IdentityUser<int>, IBaseEntity
    {
        public Employee()
        {
            this.Votes = new HashSet<Vote>();
            this.CreatedVoteSessions = new HashSet<VoteSession>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<VoteSession> CreatedVoteSessions { get; set; }
    }
}
