namespace BirthdayPresent.Core.ViewModels.VoteSession
{
    using BirthdayPresent.Core.ViewModels.Gift;
    using BirthdayPresent.Core.ViewModels.Vote;

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

        public int StatusId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? UserVotedGiftId { get; set; }

        public List<VoteViewModel> Votes { get; set; }

        public List<AllGiftsViewModel> AllGifts { get; set; }

        public Dictionary<int, int> SessionSpecificVoteCounts { get; set; }
    }
}
