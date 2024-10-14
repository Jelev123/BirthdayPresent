namespace BirthdayPresent.Core.ViewModels.VoteSession
{
    public class AllSessionsViewModel
    {
        public int Id { get; set; }

        public string BirthdaysEmployeerName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? FinishDate { get; set; }
    }
}
