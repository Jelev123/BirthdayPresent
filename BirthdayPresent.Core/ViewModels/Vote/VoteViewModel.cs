namespace BirthdayPresent.Core.ViewModels.Vote
{
    public class VoteViewModel
    {
        public int Id { get; set; }

        public int GiftId { get; set; }

        public string GiftName { get; set; }

        public int VoterId { get; set; }

        public int VoteSessionId { get; set; }
    }
}
