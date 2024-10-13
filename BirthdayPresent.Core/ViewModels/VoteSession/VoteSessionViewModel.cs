namespace BirthdayPresent.Core.ViewModels.VoteSession
{
    public class VoteSessionViewModel
    {
        public int Id { get; set; }
        public string InitiatorId { get; set; }

        public string InitiatorName { get; set; }

        public string BirthdayEmployeeId { get; set; }

        public string BirthdayEmployeeName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime VotingYear { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
