namespace BirthdayPresent.Core.ViewModels.Vote
{
    public class VoteResultViewModel
    {
        public int VoteSessionId { get; set; }

        public DateTime? FinishDate { get; set; }

        public string BirthdayEmployeeName { get; set; }

        public List<VoterViewModel> Voters { get; set; }

        public List<string> NonVoters { get; set; }

        public List<GiftResultViewModel> Gifts { get; set; }
    }
}
