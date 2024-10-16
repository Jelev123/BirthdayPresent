namespace BirthdayPresent.Core.ViewModels.VoteSession
{
    using BirthdayPresent.Core.ViewModels.Vote;

    public class AllSessionsViewModel
    {
        public int Id { get; set; }

        public string BirthdaysEmployeerName { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
