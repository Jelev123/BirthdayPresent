namespace BirthdayPresent.Infrastructure.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Votes = new HashSet<Vote>();
            this.CreatedVoteSessions = new HashSet<VoteSession>();
        }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsHasABirthDay { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<VoteSession> CreatedVoteSessions { get; set; }
    }
}
