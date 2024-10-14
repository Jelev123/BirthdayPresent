namespace BirthdayPresent.Core.ViewModels.VoteSession
{
    public class VoteSessionViewModel
    {
        public int Id { get; set; }

        public int InitiatorId { get; set; }

        public int CurrentUserId { get; set; }

        public string InitiatorName { get; set; }

        public int BirthdayEmployeeId { get; set; }

        public string BirthdayEmployeeName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int VotingYear { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
