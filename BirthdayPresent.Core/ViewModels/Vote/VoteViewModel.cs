namespace BirthdayPresent.Core.ViewModels.Vote
{
    public class VoteViewModel
    {
        public int GiftId { get; set; }

        public string VoterId { get; set; }

        public int VoteSessionId { get; set; }

        public DateTime VotedAt { get; set; }
    }
}
